ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [FK_Category_ParentCategory]
FOREIGN KEY ([ParentCategoryId])
REFERENCES [dbo].[Category]([Id]);

ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_Product_Category]
FOREIGN KEY ([CategoryId])
REFERENCES [dbo].[Category]([Id]);

ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [CK_Product_Ammount_AlwaysPositive]
CHECK ([Ammount] >= 0);