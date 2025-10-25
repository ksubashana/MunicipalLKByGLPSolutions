USE [MuniLKDbGLPSolutions]
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'PropertyTypes', N'Property Types', N'Application Main Property Types', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:08:13.7690528' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'PropertyOwnershipType', N'Property Ownership Type', N'Property Ownership Type', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:09:01.9880793' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'RoadAccessType', N'RoadAccess Type', N'RoadAccess Type', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:09:31.8019971' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'ElectoralDivision', N'Electoral Division', N'Electoral Division', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:09:57.3367971' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'DocumentType', N'Document Types', N'Application Main Document Types', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:04:17.1339283' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'9217155c-b165-4165-80ce-a4df50622035', N'LandExtentUnit', N'LandExtentUnit', N'LandExtentUnit', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:08:46.0995705' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Zone', N'Zone', N'Zone Types', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:08:35.4824481' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'ConstructionType', N'Construction Type', N'Construction Type', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:09:19.5570135' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[LookupCategories] ([Id], [Name], [DisplayName], [Description], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'GSDivision', N'GS Division', N'GS Division', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:09:45.0236693' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5c91b9b8-2e4e-4196-88ff-030260aba362', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Mawathagama (මාවතගම)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0344766' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'20004659-caad-4585-b785-09159de33179', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Residential – Single House (නිවාස – තනි නිවස)', 13, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8693599' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'0412dce4-d56e-4aef-9678-09a61984ce24', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Religious Building (ආගමික ගොඩනැගිල්ල)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7758141' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a45724d8-266a-4a33-8da9-0a5be46e543c', N'9217155c-b165-4165-80ce-a4df50622035', N'Cents (සෙන්ට්)', 10, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1564152' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'c25d0fad-43b5-4a34-a02a-0afa371e8c01', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Industrial – Heavy (කර්මාන්ත – භාරකාර කලාපය)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9764295' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'7154e0ea-d656-4840-85d2-0cf0e7997419', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Nikaweratiya (නිකවෙරටිය)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.6953010' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'4c32e2e8-06f5-4227-b3a2-0f87740f62ef', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Public / Institutional – School (පොදු / ආයතනික – පාසල)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8482817' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'e4ddd009-9b12-4a04-8359-131144f9024d', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Rambukkana (රම්බුක්කන)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.7130255' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'4f4658a2-7bd6-4914-810a-15af58aa2f65', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Mixed Development – Residential & Commercial (මැක්ස්ඩ් සංවර්ධනය – නිවාස හා වාණිජ)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8238145' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'df676cdd-3da9-4bbc-92e9-18b6ceb778c9', N'9217155c-b165-4165-80ce-a4df50622035', N'Perch (පර්ච්)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1305272' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'96e9356f-00af-48c0-9815-1a49a698ccd2', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Recreational – Park / Playground (විවේක – උයන / ක්‍රීඩාංගනය)', 16, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8814193' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'c73510b0-1879-4b8e-b65c-1cbaf65b3863', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Alley / Lane (බැන් / තාරග)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1195135' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5a1bd25c-4774-4610-a6c4-1dd6f64d5499', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Kurunegala Rural (කුරුණෑගල ගම්මහල්)', 10, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.7154987' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'f0ec543d-3e33-4eff-b090-1f32a0879b37', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Agricultural – Tea / Rubber / Coconut Plantation (කෘෂිකාර්මික – තේ / රබර් / පොල් වගාව)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8348532' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a775fecc-9845-469e-9727-2032db1b8200', N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'Inspection', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:05:32.3029966' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'1e6c94cd-5753-4d7d-9737-251aeae7613b', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Kurunegala Town (කුරුණෑගල නගරය)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0255997' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'145ec215-4776-4d7f-84ad-27202bdac954', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Kurunegala (කුරුණෑගල)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.6870988' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'7709c8ab-b2c1-4e36-a635-2a66a00bb586', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Religious / Temple Land (ආගමික / විහාර භූමිය)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8968648' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a5eedafa-c85e-471f-92ce-2df0c4dbe533', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Dead-End / Cul-de-Sac (අවසාන මාර්ගය / කල්-ඩි-සැක්)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1311256' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'79774a04-7c42-424f-ab50-3086762f87e9', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Rambukkana (රම්බුක්කන)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0474619' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'02333a5b-676e-45c2-bdcf-32e61b2ff318', N'9217155c-b165-4165-80ce-a4df50622035', N'Custom Local Measurement (සැකසුම් ලාංකීය මිනුම)', 12, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1626190' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'076a5fa1-c8f1-46eb-8be2-35f1b675ae6c', N'9217155c-b165-4165-80ce-a4df50622035', N'Rood and Perch Combination (රූඩ් සහ පර්ච් සම්බන්ධය)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1535002' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'676ffa88-c32b-46f1-80cc-36c2a6b7a504', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Industrial Building (කර්මාන්ත ගොඩනැගිල්ල)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7639648' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'd5cc545d-f9ca-4f2d-9ebb-392c9baac346', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Industrial – Light Industry / Workshop (කාර්මික – හෝල් කාර්මිකය / වැඩමුළුව)', 20, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.9015253' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'211597ac-f02a-4cc5-9443-3ab751178eb0', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Recreational – Community Hall / Center (විවේක – ප්‍රජා ශාලාව / මධ්‍යස්ථානය)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8507914' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'3e50a8ec-124b-42a5-b8cc-3b20c8d28b7d', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Industrial – Light (කර්මාන්ත – ලායිට් කලාපය)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9731016' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'b3c48917-efa8-4eae-a68f-3cf570579e2d', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Maho (මහෝ)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.7104533' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'9fcb3a00-20cc-41a8-a91f-3d64c6604fe0', N'9217155c-b165-4165-80ce-a4df50622035', N'Square Yard (වර්ග යාඩ්)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1502986' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'24c644c4-80cd-42fb-afad-3df1527d532d', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Commercial – Shop / Retail (වාණිජ – වෙළඳසල / සිල්ලර)', 11, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8557133' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'e1bf9ba5-e9ed-4cf3-8f5f-40a751a3db14', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Government Land / Reservation (රජයේ භූමිය / වෙන්කළ භූමිය)', 19, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8965496' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'6cd16cd3-b65c-4638-bd72-4345aecfd4f5', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Public / Institutional – Religious Building (පොදු / ආයතනික – ආගමික ගොඩනැගිල්ල)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8453532' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'1380c123-7bd9-4b18-b2a4-45cd1196e600', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Government / Public Building (රාජ්‍ය / පොදු ගොඩනැගිල්ල)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7728293' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'954c82e5-c159-433b-83c5-46500cf9815c', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Mixed-Use Building (මිශ්‍රිත භාවිත ගොඩනැගිල්ල)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7667482' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'add8520e-08cc-44ac-912c-4724bc985d9c', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Wariyapola (වර්‍යාපොල)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0392141' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'b8059300-0de3-4379-9349-47952b982889', N'9217155c-b165-4165-80ce-a4df50622035', N'Hectare (හෙක්ටයාර්)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1423837' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'cd3abb16-0c30-40d4-a675-4b805b94f236', N'9217155c-b165-4165-80ce-a4df50622035', N'Rood (රූඩ්)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1450254' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a75c6957-a931-44e7-b2d5-4c1e07fd2109', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Protected / Environmentally Sensitive (රක්ෂිත / පාරිසරික සංවේදී කලාපය)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9814493' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5e327da6-d153-4d7c-9dc2-53816c6ec19e', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Main Road (මූලික මාර්ගය)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1107925' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'9fabc320-cf6e-4396-b75b-539491aab0b4', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Galgamuwa (ගල්ගමුව)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.7072492' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'38cc1fe9-d6d5-4867-a6bd-548be4d9eb67', N'9217155c-b165-4165-80ce-a4df50622035', N'Perch (Decimalized) (පර්ච් - දශම)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1476185' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'6201eec3-79c9-488a-bc3f-55ff5e56e52e', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Government / State Owned (රජයේ / රාජ්‍ය අයිතිය)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8870287' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'7fef7c94-571d-44ef-8a9d-5aba6aa8cc51', N'9217155c-b165-4165-80ce-a4df50622035', N'Dunam / Local Survey Unit (ඩුනම් / දේශීය මිනුම් ඒකකය)', 11, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1595203' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'c91e188e-3006-4158-99df-5c40cf04c7b7', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Polgahawela (පොල්ගහවෙල)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.6920754' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a72fbb73-f2dc-4280-b381-5ea7cdfd5d29', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Secondary Road (දෙවන මාර්ගය)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1165479' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'df021336-b008-43d7-a6e3-6064ab05cc20', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Tourism – Guest House / Resort ( සංචාරක – ගෙස්ට් හවුස් / රිසෝර්ට්)', 18, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8916654' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'648c3257-36a3-40fc-a471-64664144839e', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Private Driveway (පෞද්ගලික ධාවන මාර්ගය)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1366371' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'feb519cf-b78b-436e-9eba-65584a760c09', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Undeveloped / Vacant Land (අවිකසිත / හිස් ඉඩම)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8373933' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'b1ab7e71-ec96-497d-8259-655985905ea4', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Residential – High Density (ඉහළ සංග්‍රහිත නිවාස කලාපය)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9563033' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'fd308189-3ec0-4634-a59a-6f96db92d1d0', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Service Road (සේවා මාර්ගය)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1339421' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'4bdac114-44a2-4b1f-9b15-714ca69d3dac', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Footpath / Pedestrian (පියවර මාර්ගය)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1223853' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'e130e0e7-7801-4fbb-b0d1-789bbf718452', N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'Vendors', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:05:32.2989256' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'470ef96e-2174-4b92-acd5-79bd86736670', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Temporary / Shed Structure (තාවකාලික / සෙඩ් ව්‍යූහය)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7787004' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5f38f770-af2a-45b6-adbf-7fd06fe6e513', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Highway / Expressway (අධිවේග මාර්ගය / ප්‍රකාශ මාර්ගය)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1283956' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'127b3387-6120-4b3b-8fd0-807109892e8b', N'9217155c-b165-4165-80ce-a4df50622035', N'Square Meter (වර්ග මීටර්)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1370743' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'773e54af-5295-4d89-a9ed-81d650b47dc6', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Public / Institutional – Hospital / Clinic (පොදු / ආයතනික – රෝහල / ඖෂධාගාර)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8273404' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'0ee3602a-57c6-476d-a6cd-85b3c53ca53a', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Agricultural – Livestock / Poultry Farm (කෘෂිකාර්මික – සතුන් / කුකුළා ගොඩනැගිලි)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8322970' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'2ad6c150-0c89-486f-a448-8837c9b115da', N'9217155c-b165-4165-80ce-a4df50622035', N'Acre (ඇකර)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1400069' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'85ec0e81-d8dd-4daf-aec3-88804777966a', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Mawathagama (මාවතගම)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.6979083' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'0985b019-9ad9-4e69-b4db-8a12df21a254', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Ibbagamuwa (ඉබ්බාගමුව)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0370044' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'c68fc77c-e7f0-4840-9d22-8cc24962356a', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Wariyapola (වර්‍යාපොල)', 6, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.7036442' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'5bf739f1-47f0-457c-89b4-8f36c5cf2f68', N'bdca3424-c6a9-4306-a3ab-8225c5cad49e', N'Private Access Road (පෞද්ගලික ප්‍රවේශ මාර්ගය)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:43.1254735' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a78e2a7c-8f67-4365-b8b2-8f6f6aa95f42', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Maho (මහෝ)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0444790' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'99944d0e-c13a-4f0b-a5ec-9bc67a635f17', N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'BuildingAndPlanning', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:05:32.2817761' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'ea86a28b-62aa-4015-b39c-9bda2bc1bfd7', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Institutional Building (ආයතනික ගොඩනැගිල්ල)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7698840' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'022b75b9-b459-4d36-8de3-a5e0462149f5', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Commercial – Office Building (වාණිජ – කාර්යාල ගොඩනැගිල්ල)', 12, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8584175' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'98965ee7-7d40-4bb4-b31e-a7e808e57bf1', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Polgahawela (පොල්ගහවෙල)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0295553' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'e5a99947-67ec-46f6-900b-abc1dc1a78b5', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Commercial – Hotel / Restaurant (වාණිජ – හෝටලය / අවන්හල)', 14, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8736441' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'236e8091-716c-48a3-bfbb-b112f29b80fc', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Agricultural (කෘෂිකාර්මික කලාපය)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9789321' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'ca348567-411d-40ec-9c3d-b38405cb2f5f', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Residential – Low Density (අඩු සංග්‍රහිත නිවාස කලාපය)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9596558' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'1b6b912a-5de1-408c-ac0c-b4287fc90530', N'9217155c-b165-4165-80ce-a4df50622035', N'Square Feet (වර්ග අඩි)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:23:48.1342071' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'b64853d9-e365-4176-af21-b924d2b87c4f', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Special Development Zone (විශේෂ සංවර්ධන කලාපය)', 12, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9936180' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'b82017dd-0016-4a2d-8d34-ba11013e16de', N'690ef4fe-c11a-495a-b4d5-9b919634f285', N'Ibbagamuwa (ඉබ්බාගමුව)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:29:12.7006365' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'3689a485-5ae3-464e-8220-baaec979bdb6', N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'Permits', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:05:32.2859711' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'818ca9e4-6ec0-444d-95e1-be0fc77fd138', N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'Photos', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:05:32.2355117' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'727c0056-6b2d-499d-aba9-caa918f384be', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Agricultural – Paddy Field (කෘෂිකාර්මික – වැව)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8298901' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'cfa1555f-56e1-4f9d-9f03-d062b74fc7ec', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Industrial – Heavy Industry (කාර්මික – දුර්වල කාර්මිකය)', 15, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8766708' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'd118c8b0-1d35-4fa6-8960-d160c98c79b4', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Provincial Council Owned (පලාත් සභා අයිතිය)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8893316' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'1040cf01-cd64-40e1-8260-d23e080340d8', N'72a18e0a-171f-4e82-8108-a32231ed8ed5', N'Construction', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T03:05:32.2899237' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'04bb41da-5b66-4479-a058-d46dfdc3853f', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Residential – Boarding / Hostel (නිවාස – බෝඩින් / නේවාසිකා)', 10, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8530893' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'6c6a9f76-4341-40ad-91c8-d78686fa57fa', N'5ebfc638-cd78-4b54-b6eb-3f3c52d7cfab', N'Residential – Apartment / Flat (නිවාස – අපාට්මන්ට් / ෆ්ලැට්)', 17, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:33:03.8867330' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'a8d9223d-dd97-4a9a-a904-d7a67dedcea9', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Institutional / Government (ආයතනික / රාජ්‍ය කලාපය)', 11, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9906719' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'ec3d2918-9912-4eb2-aaee-d837f8cdd31c', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Other (වෙනත්)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.9041584' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'61bb0ac8-4f21-4e94-ab83-ddb526867066', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Commercial – Central Business District (වණිජ – මධ්‍යම ව්‍යාපාර කලාපය)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9625824' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'c4c3e94e-e2c6-44bf-8265-de3271494411', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Kurunegala Rural (කුරුණෑගල ගම්මහල්)', 10, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0502624' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'4b2b4b8f-a581-4172-b179-e0624e46717b', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Residential Building (නිවාස ගොඩනැගිල්ල)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7561511' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'1d7d2c7e-f4e9-4666-a421-e21b78d60925', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Commercial Building (ව්‍යාපාරික ගොඩනැගිල්ල)', 2, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7610414' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'81ca5721-a25d-44b2-94ba-eab15080cc4c', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Other Construction Type (වෙනත් ගොඩනැගිල්ල වර්ග)', 10, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7845085' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'cfce95aa-596e-4cbf-81b7-eb4cf8cff622', N'2e5c2a9d-4600-47dc-b7a0-d35334ae830a', N'Renovation / Extension (නවීකරණය / දිගුව)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:21:30.7814482' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'6f595168-8d55-4d84-aab4-eb614a360b39', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Nikaweratiya (නිකවෙරටිය)', 3, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0319348' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'07e3245c-fc54-458d-8e05-eca8c261bf2a', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Cooperative / Society Owned (සහභාගි / සමාජ අයිතිය)', 5, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8945018' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'330eb4a8-a8fd-4093-8675-ed753a7852ee', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Private Leasehold (පෞද්ගලික අරමුදල් අයිතිය)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8993750' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'beba8856-bb54-49be-9765-efc214a88d8c', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Private Ownership (පෞද්ගලික අයිතිය)', 1, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8835682' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'54ea1c52-0305-436b-b9cf-f22039673207', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Urban Council / Municipal Council Owned (නගර සභා / මහ නගර සභා අයිතිය)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.8920676' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'591d3567-f9a4-4172-9735-f3f197848285', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Mixed Residential & Commercial (මිශ්‍රිත නිවාස හා වණිජ කලාපය)', 10, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9874325' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'26b16ec2-239c-4a00-84cf-f414961fdedb', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Commercial – Mixed Use (වණිජ – මිශ්‍ර භාවිතය)', 4, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9650950' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'e7ea427b-78cf-4fff-9596-f5eae3cd5eba', N'9a22cf2d-2d09-488f-a12f-c2a8c72da1a1', N'Recreational / Park (විනෝදාත්මක / උද්‍යානය)', 9, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:22:44.9841389' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'7c464fe2-d6fc-4f5f-90e6-f728aebb173d', N'2adc9b4a-a290-4df2-a935-78df48ac316a', N'Community / Village Land (ප්‍රජා / ගම් භූමිය)', 8, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:31:36.9018223' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Lookups] ([Id], [LookupCategoryId], [Value], [Order], [TenantId], [IsActive], [CreatedDate], [CreatedBy]) VALUES (N'd912c939-d10d-4167-b5a3-fb802b4fbfda', N'5530d4d3-10e0-419a-b88c-f467fdd8307e', N'Galgamuwa (ගල්ගමුව)', 7, N'00000000-0000-0000-0000-000000000001', 1, CAST(N'2025-10-18T05:19:03.0418566' AS DateTime2), N'0fedac8f-6db9-4852-b2d8-9181c94f3bea')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'd0454e72-0cb8-462e-83e8-038db01613d8', N'User & Role Management', N'USER_MGMT', N'Manage user roles, access permissions, and officers', N'fas fa-user-shield', 7, 1, NULL)
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'bf3c836b-8090-4494-af97-0608b10cbd6a', N'Permit Issuance', N'BLD_PLAN_PERMIT', N'Generate and manage construction and development permits', N'fas fa-file-signature', 4, 0, N'5877b901-83a2-4525-9b7f-a4e01b87caee')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'4450cbd0-6bff-48bc-8508-078b93c2d3a9', N'Citizen Portal', N'CITIZEN', N'Public portal for tracking applications and payments', N'fas fa-users', 5, 0, NULL)
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'a06e593f-54e6-40f3-bf24-2928454dc8e8', N'Property & Land Records', N'PROPERTY', N'Maintain property details, ownership, and zoning information', N'fas fa-home', 3, 1, NULL)
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'88e1e661-82c9-4921-8806-30619daa212b', N'Inspections & Site Visits', N'BLD_PLAN_INSPECTION', N'Manage pre-approval and post-construction inspections', N'fas fa-search-location', 3, 0, N'5877b901-83a2-4525-9b7f-a4e01b87caee')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'7e76d677-5e16-4cb0-9838-3d6c910ef72a', N'Occupancy Certificates', N'BLD_PLAN_OCCUPANCY', N'Issue certificates of conformity and occupancy approval', N'fas fa-certificate', 5, 0, N'5877b901-83a2-4525-9b7f-a4e01b87caee')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'ca3e71e7-43fc-4340-84c4-45c1d3290621', N'Infrastructure Services', N'INFRA', N'Water, drainage, road access, and utility clearances', N'fas fa-tools', 4, 0, NULL)
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'04d4a513-0629-42d7-9c8d-674c4617451e', N'Building Plan Submissions', N'BLD_PLAN_SUBMIT', N'Online submission of building plans and required documents', N'fas fa-file-upload', 1, 0, N'5877b901-83a2-4525-9b7f-a4e01b87caee')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'adb6a92c-db67-48c8-808f-771260590b35', N'Technical Reviews', N'BLD_PLAN_REVIEW', N'Technical officer and engineer plan review workflow', N'fas fa-drafting-compass', 2, 0, N'5877b901-83a2-4525-9b7f-a4e01b87caee')
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'5877b901-83a2-4525-9b7f-a4e01b87caee', N'Building & Planning', N'BLD_PLAN', N'Manage building plan submissions, reviews, and approvals', N'fas fa-building', 2, 1, NULL)
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'42a83828-5fad-4faa-8838-d39127aa2911', N'Reports & Analytics', N'REPORTS', N'Reports, dashboards, and analytics for building approvals', N'fas fa-chart-line', 6, 0, NULL)
GO
INSERT [dbo].[Modules] ([Id], [Name], [Code], [Description], [IconCssClass], [DisplayOrder], [IsCoreModule], [ParentModuleId]) VALUES (N'4ccc65aa-d034-4995-af76-df3cac8b5f15', N'Administration', N'ADMIN', N'System administration, tenant management, and configurations', N'fas fa-cogs', 1, 1, NULL)
GO
INSERT [dbo].[Tenants] ([TenantId], [Name], [Subdomain], [ContactEmail], [IsActive], [CreatedDate], [LastModifiedDate]) VALUES (N'45319738-7160-4774-93d8-256e85e5f81e', N'Matale Urban Council', N'matale', N'admin@matale.lk', 1, CAST(N'2025-10-17T04:39:42.9816590+00:00' AS DateTimeOffset), CAST(N'2025-10-17T04:39:42.9816602+00:00' AS DateTimeOffset))
GO
INSERT [dbo].[Tenants] ([TenantId], [Name], [Subdomain], [ContactEmail], [IsActive], [CreatedDate], [LastModifiedDate]) VALUES (N'bde2f8e2-dbf9-4696-a5da-4df0d3bd651d', N'Anuradhapura Municipal Council', N'anuradhapura', N'admin@anuradhapura.lk', 1, CAST(N'2025-10-17T04:39:48.4296782+00:00' AS DateTimeOffset), CAST(N'2025-10-17T04:39:48.4296791+00:00' AS DateTimeOffset))
GO
INSERT [dbo].[Tenants] ([TenantId], [Name], [Subdomain], [ContactEmail], [IsActive], [CreatedDate], [LastModifiedDate]) VALUES (N'0fedac8f-6db9-4852-b2d8-9181c94f3bea', N'Gagaawata Korale Urban Council', N'gagaawata', N'admin@gagaawata.lk', 1, CAST(N'2025-10-17T04:39:23.5534751+00:00' AS DateTimeOffset), CAST(N'2025-10-17T04:39:23.5535284+00:00' AS DateTimeOffset))
GO
INSERT [dbo].[Tenants] ([TenantId], [Name], [Subdomain], [ContactEmail], [IsActive], [CreatedDate], [LastModifiedDate]) VALUES (N'77e47be7-dd74-472f-9b29-ba3eb0fdf1cb', N'Kurunegala Municipal Council', N'kurunegala', N'admin@kurunegala.lk', 1, CAST(N'2025-10-17T04:39:33.6521117+00:00' AS DateTimeOffset), CAST(N'2025-10-17T04:39:33.6521128+00:00' AS DateTimeOffset))
GO
