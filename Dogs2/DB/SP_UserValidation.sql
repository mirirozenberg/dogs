
-- =============================================
-- Author:		Miri Rozenberg
-- Description:	Check permissions and return display Name
-- =============================================

CREATE PROCEDURE [dbo].SP_UserValidation
	@email varchar(50)
AS
	SELECT [userName] from [dbo].[Users]
		 where [userName] = @email
RETURN 0
