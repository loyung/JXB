--loyung框架基础数据库创建T-SQL语句
create database Loyung
on primary
(
name='Loyung_DATA',
filename='D:\Subjects\[9.FrameWork]\Microsoft.NET\Loyung\Loyung\App_Data\Loyung.mdf',
size=3mb,
maxsize=unlimited,
filegrowth=1mb
)
,
(
name='Loyung_SEC',
filename='D:\Subjects\[9.FrameWork]\Microsoft.NET\Loyung\Loyung\App_Data\Loyung.ndf',
size=3mb,
maxsize=unlimited,
filegrowth=1mb
)
log on
(
name='Loyung_LOG',
filename='D:\Subjects\[9.FrameWork]\Microsoft.NET\Loyung\Loyung\App_Data\Loyung.ldf',
size=1mb,
maxsize=unlimited,
filegrowth=1mb
)
--使用当前数据库
USE Loyung
go
--删除数据库
--drop database Loyung

--创建系统框架基础表

--系统模块信息--
create table SYSMA 
(
   ID                   int identity(1,1)             not null,
   MA001                varchar(50)                    null,
   MA002                varchar(50)                    null,
   MA003                varchar(200)                   null,
   MA004                varchar(20)                    null,
   MA005                int                            null,
   MA006                varchar(200)                   null,
   MA007                bit                            null,
   constraint PK_SYSMA primary key clustered (ID)   
);
--删除系统模块信息表
--drop table SYSMA
--系统功能信息--
create table SYSMB 
(
   ID                   int identity(1,1)              not null, 
   MB001                int                            null,
   MB002                varchar(50)                    null,
   MB003                varchar(50)                    null,
   MB004                bit                            null,
   constraint PK_SYSMB primary key clustered (ID),
   constraint fk_sysmb foreign key(MB001) references SYSMA(ID)--创建功能与模块外键关联
);
--删除系统功能信息
--drop table SYSMB
--系统常量字典
create table SYSDA 
(
   ID                   int identity(1,1)              not null,
   DA001                varchar(100)                   null,
   DA002                varchar(50)                    null,
   DA003                varchar(200)                   null,
   constraint PK_SYSDA primary key clustered (ID)
);
--删除系统常量字典
--drop table SYSDA
--系统字典明细
create table SYSDB 
(
   ID                   int  identity(1,1)             not null,
   DB001                int                            null,
   DB002                varchar(50)                    null,
   DB003                varchar(50)                    null,
   DB004                varchar(200)                   null,
   constraint PK_SYSDB primary key clustered (ID),
   constraint fk_sysdb foreign key(DB001) references SYSDA(ID)--创建字典明细中外键关系
);
--删除系统常量字典
--drop table SYSDB
--角色信息
create table USERA 
(
   ID                   int   identity(1,1)            not null,
   RA001                varchar(50)                    null,
   RA002                varchar(500)                   null,
   RA003                bit                            null,
   constraint PK_USERA primary key clustered (ID)
);
--删除角色信息
--drop table USERA
--角色权限分配
create table USERB 
(
   ID                   int identity(1,1)              not null,
   RB001                int                            null,
   RB002                int                            null,
   constraint PK_USERB primary key clustered (ID),
   constraint fk_rb001 foreign key(RB001) references USERA(ID),--创建角色权限分配角色外键关系
   constraint fk_rb002 foreign key(RB002) references SYSMB(ID)--创建角色权限分功能外键关系
);
--删除角色权限分配
--drop table USERB

--用户信息表
create table USEUA 
(
   ID                   bigint  identity(1,1)          not null,
   UA001                varchar(50)                    null,
   UA002                varchar(50)                    null,
   UA003                varchar(50)                    null,
   UA004                varchar(2)                     null,
   UA005                datetime                       null,
   UA006                varchar(50)                    null,
   UA007                varchar(50)                    null,
   UA008                varchar(50)                    null,
   UA009                varchar(50)                    null,
   UA010                varchar(50)                    null,
   UA011                datetime                       null,
   UA012                bit                            null,
   UA100                bit                            null,
   UA101                varchar(100)                   null,
   constraint PK_USEUA primary key clustered (ID)
);
--删除用户信息表
--drop table USEUA
--用户账户信息
create table USEUB 
(
   ID                   bigint identity(1,1)          not null,
   UB001                bigint                         null,
   UB002                varchar(50)                    null,
   UB003                varchar(200)                   null,
   constraint PK_USEUB primary key clustered (ID),
   constraint fk_useub foreign key(UB001) references USEUA(ID)--创建账户信息中外键关系
);
--删除用户账户信息
--drop table USEUB
--用户角色分配
create table USERC 
(
   ID                   int identity(1,1)              not null,
   RC001                bigint                         null,
   RC002                int                            null,
   constraint PK_USERC primary key clustered (ID),
   constraint fk_rc001 foreign key(RC001) references USEUA(ID),--创建用户角色关联-用户外键关联
   constraint fk_rc002 foreign key(RC002) references USERA(ID),--创建用户角色关联-角色外键关联
);
--删除用户角色分配
--drop table USERC
--系统标签（分类）
create table SYSTA 
(
   ID                   int identity(1,1)              not null,
   TA001                varchar(50)                    null,
   TA002                varchar(20)                    null,
   TA003                int                            null,
   TA004                varchar(200)                   null,
   constraint PK_SYSTA primary key clustered (ID)
);
--删除系统标签
--drop table SYSTA
--标签分配
create table SYSTB 
(
   ID                   int  identity(1,1)             not null,
   TB001                bigint                         null,
   TB002                int                            null,
   constraint PK_SYSTB primary key clustered (ID),
   constraint fk_tb001 foreign key(TB001) references USEUA(ID),--创建用户标签关联-用户外键关联
   constraint fk_tb002 foreign key(TB002) references SYSTA(ID),--创建用户便签关联-标签外键关联
);
--删除标签分配
--drop table SYSTB
--系统日志
create table SYSLA 
(
   ID                   bigint identity(1,1)            not null,
   LA001                varchar(1000)                  null,
   LA002                varchar(200)                   null,
   LA003                varchar(200)                   null,
   LA004                bigint                         null,
   LA005                datetime                       null,
   constraint PK_SYSLA primary key clustered (ID),
   constraint fk_la004 foreign key(LA004) references USEUA(ID)--创建用户操作日志-用户外键关联
);
--删除系统日志
--drop table SYSLA
go
