﻿using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class SxRepo301Redirect<TDbContext> : SxDbRepository<Guid, Sx301Redirect, TDbContext> where TDbContext : SxDbContext
    {
        public override SxVM301Redirect[] Query<SxVM301Redirect>(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dr.Id", "dr.OldUrl", "dr.NewUrl", "dr.DateCreate" });
            query += " FROM   D_REDIRECT dr";

            object param = null;
            query += get301RedirectWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dr.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxVM301Redirect>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM   D_REDIRECT dr";

            object param = null;
            query += get301RedirectWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<int>(query, param: param).Single();
            }
        }

        private static string get301RedirectWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dr.OldUrl LIKE '%'+@old_url+'%' OR @old_url IS NULL)";
            query += " AND (dr.NewUrl LIKE '%'+@new_url+'%' OR @new_url IS NULL)";

            var oldUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.OldUrl != null ? (string)filter.WhereExpressionObject.OldUrl : null;
            var newUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NewUrl != null ? (string)filter.WhereExpressionObject.NewUrl : null;

            param = new
            {
                old_url = oldUrl,
                new_url = newUrl
            };

            return query;
        }

        /// <summary>
        /// Редирект страницы
        /// </summary>
        /// <returns></returns>
        public Sx301Redirect Get301Redirect(string url)
        {
            Sx301Redirect result = new Sx301Redirect();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<Sx301Redirect>("get_redirect @url", new { url= url }).SingleOrDefault();
                if (data != null)
                    result = data;
            }

            return result;
        }
    }
}