CREATE TABLE [dbo].[ShoppingCartItem] (
    [Id]                 INT            NOT NULL,
    [ShoppingCartId]     INT            NOT NULL,
    [ProductCatalogId]   BIGINT         NOT NULL,
    [ProductName]        NVARCHAR (100) NOT NULL,
    [ProductDescription] NVARCHAR (500) NULL,
    [Amount]             INT            NOT NULL,
    [Currency]           NVARCHAR (5)   DEFAULT ('USD') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Item_Cart] FOREIGN KEY ([ShoppingCartId]) REFERENCES [dbo].[ShoppingCart] ([Id])
);

