INSERT INTO public.sys_users_roles ("UserId", "RoleId") VALUES((select "Id" from public.sys_users where "UserName" = 'allen' limit 1), 1);