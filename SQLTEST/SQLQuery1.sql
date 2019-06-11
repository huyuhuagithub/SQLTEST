--SELECT [id],[Account],[Password],[Permission] FROM [users] where Permission=1 and Account='aptk'

--SELECT [id],[Account],[Password],[Permission] FROM [users] where Permission=1 order by Id asc

--SELECT SUM(ID) FROM [users]	
--SELECT AVG(ID) FROM [users]	
--SELECT COUNT(ID) FROM [users]
--SELECT MAX(ID) FROM [users]
SELECT MIN(ID) FROM	[users]