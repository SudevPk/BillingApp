--DROP DATABASE [faaDB]
--CREATE Database
CREATE DATABASE faa

--CREATE Tables
 --Sales
create table T_D_SALES(
sales_id int identity(1,1),
sales_date datetime default getdate(),
customer_id int not null, 
extra_discount_cash numeric(10,2) default 0,
extra_discount_perc numeric(10,2) default 0,
total_amnt numeric(20,2),
amnt_paid numeric(20,2),
current_sales_balance numeric(20,2),
created_date datetime default getdate(),
updated_date datetime default getdate(),
is_delete int default 0
)
 --Sales Items
create table T_D_SALES_ITEMS(
sales_item_id int identity(1,1),
sales_id int,
item_code varchar(50) not null,
item_name varchar(50) not null,
item_qty int default 1,
item_price_per_piece numeric(10,2),
item_discount numeric(10,2),
s_gst numeric(10,2),
c_gst numeric(10,2),
total numeric(10,2),
total_per_item numeric(10,2),
created_date datetime default getdate(),
updated_date datetime default getdate(),
is_delete int default 0,
)
 --Product
create table M_S_PRODUCT(
product_id int identity (1,1),
product_code varchar(10) not null,
product_name varchar(100) not null,
special_discount_cash numeric(10,2) default 0,
special_discount_perc numeric(10,2) default 0,
increase_amnt_by_cash numeric(10,2) default 0,
increase_amnt_by_perc numeric(10,2) default 0,
created_date datetime default getdate(),
updated_date datetime default getdate(),
is_delete int default 0,
primary key(
product_code
) 
)
 --Customers
create table M_S_CUSTOMERS(
cust_id int identity(1,1) ,
customer_name varchar(100),
customer_city varchar(100),
customer_state varchar(100),
customer_district varchar(100),
customer_phone varchar(10) not null,
customer_email varchar(100),
customer_address varchar(255),
special_discount_cash numeric(10,2),
special_discount_perc numeric(10,2),
created_date datetime default getdate(),
updated_date datetime default getdate(),
is_delete int default 0,
primary key(
customer_phone
)
) 