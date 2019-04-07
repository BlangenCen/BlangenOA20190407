select * from ActionInfo

insert into ActionInfo([SubTime], [DelFlag], [ModifiedOn], [Remark], [Url], [HttpMethod], [ActionMethodName], [ControllerName], [ActionInfoName], [Sort], [ActionTypeEnum], [MenuIcon], [IconWidth], [IconHeight])
select [SubTime], [DelFlag], [ModifiedOn], [Remark], [Url], [HttpMethod], [ActionMethodName], [ControllerName], [ActionInfoName], [Sort], [ActionTypeEnum], [MenuIcon], [IconWidth], [IconHeight] from ActionInfo where ID=29


update ActionInfo set url = LOWER(url) 

select * from ActionInfo

GetRoleInfoList
DeleteRoleInfo
ShowAdd
AddRoleInfo
FindRoleInfo
ShowEdit
EditRoleInfo
ShowRoleAction
SetRoleAction
update ActionInfo set ActionTypeEnum = 0  where id >= 30

update ActionInfo set Remark = 'GetRoleInfoList', url = LOWER('/roleinfo/GetRoleInfoList'), ActionInfoName = 'GetRoleInfoList', ActionTypeEnum = 0  where id = 40
update ActionInfo set Remark = 'DeleteRoleInfo', url = LOWER('/roleinfo/DeleteRoleInfo'), ActionInfoName = 'DeleteRoleInfo' , ActionTypeEnum = 0  where id = 41
update ActionInfo set Remark = 'ShowAdd', url = LOWER('/roleinfo/ShowAdd'), ActionInfoName = 'ShowAdd' , ActionTypeEnum = 0  where id = 42
update ActionInfo set Remark = 'AddRoleInfo', url = LOWER('/roleinfo/AddRoleInfo'), ActionInfoName = 'AddRoleInfo' , ActionTypeEnum = 0  where id = 43
update ActionInfo set Remark = 'FindRoleInfo', url = LOWER('/roleinfo/FindRoleInfo'), ActionInfoName = 'FindRoleInfo' , ActionTypeEnum = 0  where id = 44
update ActionInfo set Remark = 'ShowEdit', url = LOWER('/roleinfo/ShowEdit'), ActionInfoName = 'ShowEdit' , ActionTypeEnum = 0  where id = 45
update ActionInfo set Remark = 'EditRoleInfo', url = LOWER('/roleinfo/EditRoleInfo'), ActionInfoName = 'EditRoleInfo' , ActionTypeEnum = 0  where id = 40
update ActionInfo set Remark = 'ShowRoleAction', url = LOWER('/roleinfo/ShowRoleAction'), ActionInfoName = 'ShowRoleAction', ActionTypeEnum = 0   where id = 46
update ActionInfo set Remark = 'SetRoleAction', url = LOWER('/roleinfo/SetRoleAction'), ActionInfoName = 'SetRoleAction' , ActionTypeEnum = 0  where id = 47


update ActionInfo set Remark = 'GetActionInfoList', url = LOWER('/ActionInfo/GetActionInfoList'), ActionInfoName = 'GetActionInfoList' , ActionTypeEnum = 0  where id = 48

update ActionInfo set HttpMethod = 'GET' where id = 34

update ActionInfo set HttpMethod = 'POST' where id between 19 and 48

select *,num = row_number() over(order by subtime) from ActionInfo 