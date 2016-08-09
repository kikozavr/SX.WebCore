﻿using Microsoft.AspNet.Identity.EntityFramework;
using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace SX.WebCore
{
    public class SxDbContext : IdentityDbContext<SxAppUser>
    {
        //Строка поделючения задается здесь
        private static string _nameOrConnectionString;
        public SxDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) 
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public static TDbContext Create<TDbContext>()
        {
            var context = Activator.CreateInstance<TDbContext>();
            return context;
        }

        public DbSet<SxArticle> Articles { get; set; }

        public DbSet<SxAffiliateLink> AffiliateLinks { get; set; }

        public DbSet<SxBannedUrl> BannedUrls { get; set; }

        public DbSet<SxBanner> Banners { get; set; }

        public DbSet<SxBannerGroup> BannerGroups { get; set; }

        public DbSet<SxComment> Comments { get; set; }

        public DbSet<SxEmployee> Employees { get; set; }

        public DbSet<SxHumor> Humors { get; set; }

        public DbSet<SxNet> Nets { get; set; }

        public DbSet<SxNews> News { get; set; }

        public DbSet<SxPicture> Pictures { get; set; }

        public DbSet<SxProjectStep> ProjectSteps { get; set; }

        public DbSet<Sx301Redirect> Redirects { get; set; }

        public DbSet<SxRequest> Requestes { get; set; }

        public DbSet<SxSeoTags> SeoInfo { get; set; }

        public DbSet<SxSeoKeyword> SeoKeyWords { get; set; }

        public DbSet<SxShareButton> ShareButtons { get; set; }

        public DbSet<SxSiteSetting> SiteSettings { get; set; }

        public DbSet<SxSiteTest> SiteTests { get; set; }

        public DbSet<SxSiteTestQuestion> SxSiteTestQuestions { get; set; }

        public DbSet<SxStatistic> Statistic { get; set; }

        public DbSet<SxVideo> Videos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SxMaterial>().HasKey(x => new { x.Id, x.ModelCoreType });

            modelBuilder.Entity<SxSiteSetting>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SxComment>().HasRequired(x => x.Material).WithMany().HasForeignKey(x => new { x.MaterialId, x.ModelCoreType }).WillCascadeOnDelete();
            modelBuilder.Entity<SxRating>().HasRequired(x => x.Material).WithMany().HasForeignKey(x => new { x.MaterialId, x.ModelCoreType }).WillCascadeOnDelete();

            modelBuilder.Entity<SxMaterialTag>().HasKey(x=> new { x.Id, x.MaterialId, x.ModelCoreType }).HasRequired(x => x.Material).WithMany().HasForeignKey(x => new { x.MaterialId, x.ModelCoreType }).WillCascadeOnDelete();
            modelBuilder.Entity<SxMaterialTag>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SxSeoTags>().HasOptional(x => x.Material).WithMany().HasForeignKey(x => new { x.MaterialId, x.ModelCoreType });

            modelBuilder.Entity<SxMaterialCategory>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SxUserClick>().HasOptional(x => x.User).WithMany().HasForeignKey(x => new { x.UserId });
            modelBuilder.Entity<SxUserClick>().HasRequired(x => x.Material).WithMany().HasForeignKey(x => new { x.MaterialId, x.ModelCoreType });

            modelBuilder.Entity<SxLike>().HasRequired(x => x.UserClick).WithMany().HasForeignKey(x => new { x.UserClickId });

            modelBuilder.Entity<SxStatisticUserLogin>().HasKey(x => new { x.StatisticId, x.UserId });

            modelBuilder.Entity<SxBannerGroupBanner>().HasKey(x => new { x.BannerId, x.BannerGroupId });

            modelBuilder.Entity<SxVideoLink>().HasKey(x => new { x.MaterialId, x.ModelCoreType, x.VideoId });
            modelBuilder.Entity<SxVideoLink>().HasRequired(x => x.Material).WithMany(x => x.VideoLinks).HasForeignKey(x => new { x.MaterialId, x.ModelCoreType }).WillCascadeOnDelete(true);
            modelBuilder.Entity<SxVideoLink>().HasRequired(x => x.Video).WithMany().HasForeignKey(x => new { x.VideoId }).WillCascadeOnDelete(true);

            modelBuilder.Entity<SxEmployee>().HasKey(x => x.Id).HasRequired(x=>x.User).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<SxBanner>().Property(x => x.TargetCost).HasColumnType("money");
            modelBuilder.Entity<SxBanner>().Property(x => x.CPM).HasColumnType("money");
            modelBuilder.Entity<SxAffiliateLink>().Property(x => x.ClickCost).HasColumnType("money");

            modelBuilder.Entity<SxSiteTestAnswer>().HasKey(x => new { x.QuestionId, x.SubjectId });
            modelBuilder.Entity<SxSiteTestAnswer>().HasRequired(x => x.Question).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SxSiteTestAnswer>().HasRequired(x => x.Subject).WithMany().WillCascadeOnDelete(false);
        }
    }
}
