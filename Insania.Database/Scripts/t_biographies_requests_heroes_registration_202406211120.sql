INSERT INTO public.re_biographies_requests_heroes_registration (request_id, biography_id, decision, "comment", date_create, user_create, date_update, user_update, date_deleted, is_system) VALUES((select r.id from public.sys_users su join public.re_players p on su."Id" = p.user_id join public.re_heroes h on p.id = h.player_id join public.re_requests_heroes_registration r on h.id = r.hero_id where su."UserName" = 'allen' and h.personal_name = 'Дартаара' and h.family_name = 'Миркраниис' and r.status_id = 1 limit 1), (select b.id from public.sys_users su join public.re_players p on su."Id" = p.user_id join public.re_heroes h on p.id = h.player_id join public.re_biographies_heroes b on h.id = b.hero_id where su."UserName" = 'allen' and h.personal_name = 'Дартаара' and h.family_name = 'Миркраниис' and b.day_begin = 16 and b.month_begin_id = 1 and b.cycle_begin = 1761 limit 1), NULL, NULL, '2024-06-12 15:43:16.521', 'allen', '2024-06-12 15:43:16.521', 'allen', NULL, false);