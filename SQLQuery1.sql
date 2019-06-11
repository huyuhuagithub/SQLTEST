
----计算函数
--select sum(id) from [users]
--select MIN(id) from [users]
--select AVG(id) from [users]
--select COUNT(id) from [users]

----增加
--insert [users] values (1008,N'APTK',1232323,1)
insert [users] (Id,Account,Permission,Password) values(1006,N'映台',1,121212)
----删除
--delete [users] where Id=1008
----修改
--update [users] set Permission=0,Password=10001212,Account=N'aptk' where Id=1005
update [users] set Account=N'操作员',Permission=0,Password=123 where id=1006
--查询
select [users].Id,[users].Account,users.Password,users.Permission from [users] where Permission=1



