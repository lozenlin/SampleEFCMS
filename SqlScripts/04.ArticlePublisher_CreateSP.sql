-- Article publisher SP, DB Functions
-- use SampleEFCMS
go

-- db functions
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/01>
-- Description: <網頁內容在指定語系是否顯示>
-- Test:
/*
select dbo.fnArticle_IsShowInLang('00000000-0000-0000-0000-000000000000', 'zh-TW')
*/
-- =============================================
create function dbo.fnArticle_IsShowInLang(
@ArticleId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select 
		@IsShowInLang=IsShowInLang
	from dbo.ArticleMultiLang
	where ArticleId=@ArticleId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/07>
-- Description: <附件檔案在指定語系是否顯示>
-- Test:
/*
*/
-- =============================================
create function dbo.fnAttachFile_IsShowInLang(
@AttId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select
		@IsShowInLang=IsShowInLang
	from dbo.AttachFileMultiLang
	where AttId=@AttId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/11>
-- Description: <網頁照片在指定語系是否顯示>
-- Test:
/*
select dbo.fnArticlePicture_IsShowInLang('C2FC6EE9-D018-4A0C-B927-3362DDB5D902', 'zh-TW')
*/
-- =============================================
create function dbo.fnArticlePicture_IsShowInLang(
@PicId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select
		@IsShowInLang=IsShowInLang
	from dbo.ArticlePictureMultiLang
	where PicId=@PicId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2017/12/13>
-- Description: <網頁影片在指定語系是否顯示>
-- Test:
/*
select dbo.fnArticleVideo_IsShowInLang('68480C66-8F2F-4CEB-BC21-CD770B34B2F4', 'zh-TW')
*/
-- =============================================
create function dbo.fnArticleVideo_IsShowInLang(
@VidId uniqueidentifier
,@CultureName varchar(10)
)
returns bit
as
begin
	declare @IsShowInLang bit

	select
		@IsShowInLang=IsShowInLang
	from dbo.ArticleVideoMultiLang
	where VidId=@VidId
		and CultureName=@CultureName

	return isnull(@IsShowInLang, 0)
end
go

-- =============================================
-- Author:		<http://lazycoders.blogspot.tw/2007/06/stripping-html-from-text-in-sql-server.html>
-- Create date: <2018/01/08>
-- Description:	<移除Html碼>
-- Test:
/*
*/
-- =============================================
create function dbo.fnStripHTML
(
@HTMLText nvarchar(MAX)
)
returns nvarchar(MAX)
as
begin
	DECLARE @Start  int
	DECLARE @End    int
	DECLARE @Length int

	SET @Start = CHARINDEX('<script', @HTMLText)
	SET @End = CHARINDEX('</script>', @HTMLText, CHARINDEX('<script', @HTMLText))
	SET @Length = (@End - @Start) + 9

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
		SET @Start = CHARINDEX('<script', @HTMLText)
		SET @End = CHARINDEX('</script>', @HTMLText, CHARINDEX('<script', @HTMLText))
		SET @Length = (@End - @Start) + 9
	END

	SET @Start = CHARINDEX('<style', @HTMLText)
	SET @End = CHARINDEX('</style>', @HTMLText, CHARINDEX('<style', @HTMLText))
	SET @Length = (@End - @Start) + 8

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
		SET @Start = CHARINDEX('<style', @HTMLText)
		SET @End = CHARINDEX('</style>', @HTMLText, CHARINDEX('<style', @HTMLText))
		SET @Length = (@End - @Start) + 8
	END

	-- Replace the HTML entity &amp; with the '&' character (this needs to be done first, as
	-- '&' might be double encoded as '&amp;amp;')
	SET @Start = CHARINDEX('&amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
		SET @Start = CHARINDEX('&amp;', @HTMLText)
		SET @End = @Start + 4
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &lt; with the '<' character
	SET @Start = CHARINDEX('&lt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '<')
		SET @Start = CHARINDEX('&lt;', @HTMLText)
		SET @End = @Start + 3
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &gt; with the '>' character
	SET @Start = CHARINDEX('&gt;', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '>')
		SET @Start = CHARINDEX('&gt;', @HTMLText)
		SET @End = @Start + 3
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &amp; with the '&' character
	SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '&')
		SET @Start = CHARINDEX('&amp;amp;', @HTMLText)
		SET @End = @Start + 4
		SET @Length = (@End - @Start) + 1
	END

	-- Replace the HTML entity &nbsp; with the ' ' character
	SET @Start = CHARINDEX('&nbsp;', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, ' ')
		SET @Start = CHARINDEX('&nbsp;', @HTMLText)
		SET @End = @Start + 5
		SET @Length = (@End - @Start) + 1
	END

	-- Replace any <br> tags with a newline
	SET @Start = CHARINDEX('<br>', @HTMLText)
	SET @End = @Start + 3
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, 'CHAR(13) + CHAR(10)')	--本來 CHAR(13) + CHAR(10) 前後沒加單引號
		SET @Start = CHARINDEX('<br>', @HTMLText)
		SET @End = @Start + 3
		SET @Length = (@End - @Start) + 1
	END

	-- Replace any <br/> tags with a newline
	SET @Start = CHARINDEX('<br/>', @HTMLText)
	SET @End = @Start + 4
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, 'CHAR(13) + CHAR(10)')
		SET @Start = CHARINDEX('<br/>', @HTMLText)
		SET @End = @Start + 4
		SET @Length = (@End - @Start) + 1
	END

	-- Replace any <br /> tags with a newline
	SET @Start = CHARINDEX('<br />', @HTMLText)
	SET @End = @Start + 5
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, 'CHAR(13) + CHAR(10)')
		SET @Start = CHARINDEX('<br />', @HTMLText)
		SET @End = @Start + 5
		SET @Length = (@End - @Start) + 1
	END

	-- Remove anything between <whatever> tags
	SET @Start = CHARINDEX('<', @HTMLText)
	SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
	SET @Length = (@End - @Start) + 1

	WHILE (@Start > 0 AND @End > 0 AND @Length > 0) BEGIN
		SET @HTMLText = STUFF(@HTMLText, @Start, @Length, '')
		SET @Start = CHARINDEX('<', @HTMLText)
		SET @End = CHARINDEX('>', @HTMLText, CHARINDEX('<', @HTMLText))
		SET @Length = (@End - @Start) + 1
	END

	RETURN LTRIM(RTRIM(@HTMLText))

end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- Description: <建立搜尋結果的足跡用資料>
-- Test:
/*
select dbo.fnBuildBreadcrumbData('759CE382-7669-48A2-8B0E-230F65597AC3', 'zh-TW')
select dbo.fnBuildBreadcrumbData('00000000-0000-0000-0000-000000000000', 'zh-TW')
*/
-- =============================================
create function dbo.fnBuildBreadcrumbData
(
@ArticleId uniqueidentifier
,@CultureName varchar(10)
)
returns nvarchar(4000)
begin
	declare @BreadcrumbData nvarchar(4000)
	declare @ArticleSubject nvarchar(200)

	--先加自己
	select
		@ArticleSubject=am.ArticleSubject
	from dbo.Article a
		left join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId
	where a.ArticleId=@ArticleId 
		and am.CultureName=@CultureName
	
	set @BreadcrumbData = @ArticleSubject+','+cast(@ArticleId as varchar(36))

	declare @ParentId uniqueidentifier
	declare @newParenId uniqueidentifier
	--取得上層網頁代碼
	select @ParentId=ParentId
	from dbo.Article
	where ArticleId=@ArticleId

	--一個一個往前抓
	while @ParentId is not null and @ParentId<>'00000000-0000-0000-0000-000000000000'
	begin
		select
			@ArticleSubject=am.ArticleSubject
		from dbo.Article a
			left join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId 
		where a.ArticleId=@ParentId
			and am.CultureName=@CultureName
	
		set @BreadcrumbData=@ArticleSubject+','+cast(@ParentId as varchar(36))+','+@BreadcrumbData

		--取得上層網頁代碼
		set @newParenId=null
		
		select @newParenId=ParentId
		from dbo.Article
		where ArticleId=@ParentId
		
		set @ParentId=@newParenId
	end

	return @BreadcrumbData
end
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- Description: <取得階層1的網頁代碼>
-- Test:
/*
select dbo.fnGetLv1ArticleId('759CE382-7669-48A2-8B0E-230F65597AC3')
select dbo.fnGetLv1ArticleId('00000000-0000-0000-0000-000000000000')
*/
-- =============================================
create function dbo.fnGetLv1ArticleId
(
@ArticleId uniqueidentifier
)
returns uniqueidentifier
begin
	declare @Lv1Id uniqueidentifier
	set @Lv1Id=@ArticleId

	declare @ArticleLevelNo int
	--取得目前階層
	select @ArticleLevelNo=ArticleLevelNo
	from dbo.Article
	where ArticleId=@ArticleId

	--一個一個往前抓
	while @ArticleLevelNo>1
	begin
		select @Lv1Id=ParentId
		from dbo.Article
		where ArticleId=@Lv1Id

		set @ArticleLevelNo=@ArticleLevelNo-1
	end
	
	return @Lv1Id
end
go

-- =============================================
-- Author:		<lozen_lin>
-- Create date: <2018/01/09>
-- Description:	<字串串接轉為表格>
/*
select * from dbo.fnStringToTable(N',', N'xxx')
select * from  dbo.fnStringToTable(N',', N'aaa,bbb,')
select * from  dbo.fnStringToTable(N',', N'aaa , bbb , c')
*/
-- =============================================
create function dbo.fnStringToTable
(
@SplitterSymbol nvarchar(100)	= N','
,@String nvarchar(4000)
)
returns @tblResult table(
	Token nvarchar(4000)
)
as
begin
	declare @iSplitter int = charindex(@SplitterSymbol, @String)

	if @iSplitter=0
	begin
		insert into @tblResult(Token)
		values(@String)

		return
	end

	declare @Token nvarchar(4000)
	declare @iStart int = 1
	declare @iEnd int = @iSplitter-1

	while @iStart <= len(@String)
	begin
		set @Token = substring(@String, @iStart, @iEnd-@iStart+1)

		insert into @tblResult(Token)
		values (ltrim(rtrim(@Token)))

		if @iEnd = len(@String)
		begin
			break
		end

		set @iStart = @iSplitter+1
		set @iSplitter = charindex(@SplitterSymbol, @String, @iStart)	--超過總長度也會回傳0
		set @iEnd = @iSplitter-1

		if @iSplitter=0
		begin
			set @iEnd = len(@String)
		end
	end

	return
end
go

-- sp
go
----------------------------------------------------------------------------
-- 搜尋用資料來源
----------------------------------------------------------------------------
go

-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/01/08>
-- History:
--	2018/01/31, lozen_lin, modify, 列出被父層隱藏的子網頁
-- Description: <建立搜尋用資料來源>
-- Test:
/*
*/
-- =============================================
create procedure dbo.spSearchDataSource_Build
@MainLinkUrl	nvarchar(2048) = N''
as
begin
	if @MainLinkUrl=N''
	begin
		set @MainLinkUrl = N'~/Article.aspx'
	end

	--刪除原本資料
	truncate table dbo.SearchDataSource
	
	--建立網頁資料
	insert dbo.SearchDataSource(
		ArticleId, CultureName, ArticleSubject, 
		ArticleContext, ReadCount, 
		LinkUrl, 
		PublishDate, 
		BreadcrumbData, Lv1ArticleId, PostDate)
		select
			a.ArticleId, am.CultureName, am.ArticleSubject, 
			/*dbo.fnStripHTML(am.ArticleContext)*/ am.TextContext, am.ReadCount, 
			case a.ShowTypeId when 3/*URL*/ then a.LinkUrl else @MainLinkUrl end as LinkUrl, 
			a.PublishDate, 
			dbo.fnBuildBreadcrumbData(a.ArticleId, am.CultureName), dbo.fnGetLv1ArticleId(a.ArticleId), getdate()
		from dbo.Article a
			join dbo.ArticleMultiLang am on a.ArticleId=am.ArticleId
			join dbo.Article p on a.ParentId=p.ArticleId
		where a.IsHideSelf=0 
			and am.IsShowInLang=1 
			and a.StartDate <= getdate() and getdate() < a.EndDate+1

	--刪除不需要的資料
	delete s
	from dbo.SearchDataSource s
	where s.ArticleSubject=''
		or (isnull(s.ArticleContext,'')='' 
			and not exists(select * from dbo.AttachFile
						where ArticleId=s.ArticleId) --留下有附件的空文章
			)
		or s.ArticleId='00000000-0000-0000-0000-000000000000'
end
go



/*

----------------------------------------------------------------------------
-- xxxxx
----------------------------------------------------------------------------
go
-- =============================================
-- Author:      <lozen_lin>
-- Create date: <2018/02/08>
-- Description: <xxxxxxxxxxxxxxxxxx>
-- Test:

-- =============================================
create procedure xxxxx

as
begin

end
go

*/
