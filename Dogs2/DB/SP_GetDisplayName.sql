
/****** Object:  StoredProcedure [dbo].[SP_GetDisplayName]    Script Date: 05/02/2020 9:35:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Miri Rozenberg
-- Description:	Check I user Exist
-- =============================================
alter PROCEDURE [dbo].[SP_UserValidation]
	-- Add the parameters for the stored procedure here
	@email varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [userName] from [dbo].[Users]
		 where [userName] = @email
END
GO

