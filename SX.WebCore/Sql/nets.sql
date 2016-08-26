IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='vk')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('vk',N'Вконтакте','http://vk.com','fa fa-vk','#45668e',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='fb')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('fb',N'Facebook','http://facebook.com','fa fa-facebook','#3b5998',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='ok')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('ok',N'Одноклассники','http://ok.ru','fa fa-odnoklassniki','#ed812b',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='mr')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('mr',N'Мой Мир@Mail.Ru','http://my.mail.ru','fa','#168de2',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='gp')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('gp',N'Google Plus','http://lus.google.com','fa fa-google-plus','#dd4b39',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='li')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('li',N'LinkedIn','http://linkedin.com','fa fa-linkedin','#0976b4',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='tw')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('tw',N'Twitter','http://twitter.com','fa fa-twitter','#55acee',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='lj')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('lj',N'LiveJournal','http://livejournal.com','fa','#004359',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='tm')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('tm',N'tumblr','http://tumblr.com','fa fa-tumblr','#35465c',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='bl')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('bl',N'Blogger','http://blogger.com','fa','#f57d00',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='pt')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('pt',N'Pinterest','http://pinterest.com','fa fa-pinterest-p','#cc2127',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='di')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('di',N'Digg','http://digg.com','fa fa-digg','#000000',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='en')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('en',N'Evernote','http://evernote.com','fa','#7ac142',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='rd')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('rd',N'Reddit','http://reddit.com','fa fa-reddit','#5f99cf',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='de')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('de',N'Delicious','http://delicious.com','fa fa-delicious','#3399ff',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='su')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('su',N'StumbleUpon','http://stumbleupon.com','fa fa-stumbleupon','#eb4924',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='po')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('po',N'Pocket','http://getpocket.com','fa fa-get-pocket','#d3505a',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='sb')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('sb',N'Surfingbird','http://surfingbird.ru','fa','#26B1F6',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='lr')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('lr',N'LiveInternet','http://liveinternet.ru','fa','#000000',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='bf')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('bf',N'Buffer','http://buffer.com','fa','#323b43',1,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='ip')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('ip',N'Instapaper','http://instapaper.com','fa','#428bca',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='ra')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('ra',N'Readability','http://readability.com','fa','#990000',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='xi')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('xi',N'Xing','http://xing.com','fa fa-xing','#cfdc00',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='wp')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('wp',N'WordPress','http://wordpress.com','fa fa-wordpress','#0087be',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='bd')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('bd',N'Baidu','http://baidu.com','fa','#2529d8',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='rr')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('rr',N'Renren','http://renren.com','fa fa-renren','#53a9d7',0,GETDATE());

IF NOT EXISTS (SELECT*FROM [D_NET] WHERE Code='wb')
INSERT INTO [dbo].[D_NET]([Code],[Name],[Url],[LogoCssClass],[Color],[HasCounter],[DateCreate])
VALUES('wb',N'Weibo','http://weibo.com','fa fa-weibo','#c53220',0,GETDATE());

DECLARE @id INT
DECLARE c CURSOR LOCAL FORWARD_ONLY FAST_FORWARD FOR
SELECT dn.Id FROM D_NET AS dn
OPEN c
FETCH NEXT FROM c INTO @id
WHILE @@FETCH_STATUS=0
BEGIN
	IF NOT EXISTS (SELECT*FROM D_SHARE_BUTTON AS dlb WHERE dlb.NetId=@id)
	INSERT INTO D_SHARE_BUTTON
	(
		NetId,
		Show,
		ShowCounter,
		DateUpdate,
		DateCreate
	)
	VALUES
	(
		@id,
		0,
		0,
		GETDATE(),
		GETDATE()
	)
	
	FETCH NEXT FROM c INTO @id
END
CLOSE c
DEALLOCATE c