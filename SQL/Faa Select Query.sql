--Select Data From Tables
select * from T_D_SALES	with(nolock)

select * from T_D_SALES_ITEMS with(nolock)

select * from M_S_PRODUCT with(nolock)

select * from M_S_CUSTOMERS	with(nolock)

select cust_id,customer_name,customer_phone,customer_email,customer_address from M_S_CUSTOMERS where customer_name+'(' +customer_phone+')' ='Sudev(8122341317)'

select MAX(sales_id) sales_id from T_D_SALES

TRUNCATE TABLE [T_D_SALES]

TRUNCATE TABLE T_D_SALES_ITEMS


select  sales_date,cust_id,customer_name,customer_phone,customer_email,customer_address from [T_D_SALES] INNER JOIN M_S_CUSTOMERS ON customer_id=cust_id where sales_id=1


select item_name as Item,item_qty as Quantity,item_price_per_piece as RatePerItem,total as Total,c_gst+s_gst as GSTRate,0 as Discount,0 as TotalAmount from T_D_SALES_ITEMS SI INNER JOIN T_D_SALES S on S.sales_id=SI.sales_id where S.sales_id=1


select total_amnt,amnt_paid,current_sales_balance from T_D_SALES where sales_id=1

select product_code+' - '+product_name from M_S_PRODUCT


select customer_name+'('+customer_phone+')' as customer_name,cust_id from M_S_CUSTOMERS where isNull(is_delete,0) = 0




