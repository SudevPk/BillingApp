USE [faaNew]
GO

INSERT INTO [dbo].[M_S_PRODUCT]
           ([product_code]
           ,[product_name]
           ,[special_discount_cash]
           ,[special_discount_perc]
           ,[increase_amnt_by_cash]
           ,[increase_amnt_by_perc]
           ,[created_date]
           ,[updated_date]
           ,[is_delete])
     VALUES
           ('ST'
           ,'SOFT TISSUE'
           ,0
           ,0
           ,0
           ,0
           ,GETDATE()
           ,GETDATE()
           ,0)
GO


INSERT INTO [dbo].[M_S_PRODUCT]
           ([product_code]
           ,[product_name]
           ,[special_discount_cash]
           ,[special_discount_perc]
           ,[increase_amnt_by_cash]
           ,[increase_amnt_by_perc]
           ,[created_date]
           ,[updated_date]
           ,[is_delete])
     VALUES
           ('HT'
           ,'HARD TISSUE'
           ,0
           ,0
           ,0
           ,0
           ,GETDATE()
           ,GETDATE()
           ,0)
GO
