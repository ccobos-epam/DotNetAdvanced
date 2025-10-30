INSERT INTO [Category]
([Id], [Name], [ParentCategoryId])
VALUES
('019a1b8c-9988-77ab-bd38-ecf2ccb23cc5', 'Tools', NULL),
('019a1b8c-9988-77ab-bd38-ecf2ccb23cc6', 'Specialized Tools', '019a1b8c-9988-77ab-bd38-ecf2ccb23cc5');

INSERT INTO [Product]
([Id], [Name], [Price], [Ammount], [CategoryId])
VALUES
('019a1b90-4a86-7155-9095-da2e6b8812a2', 'Drill', 500, 2, '019a1b8c-9988-77ab-bd38-ecf2ccb23cc5'),
('019a1b90-4a86-7fd5-a47a-902e74588a37', 'Impact Drill', 750, 1, '019a1b8c-9988-77ab-bd38-ecf2ccb23cc6');