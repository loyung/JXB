--loyung��ܻ������ݿⴴ��T-SQL���
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
--ʹ�õ�ǰ���ݿ�
USE Loyung
go
--ɾ�����ݿ�
--drop database Loyung

--����ϵͳ��ܻ�����

--ϵͳģ����Ϣ--
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
--ɾ��ϵͳģ����Ϣ��
--drop table SYSMA
--ϵͳ������Ϣ--
create table SYSMB 
(
   ID                   int identity(1,1)              not null, 
   MB001                int                            null,
   MB002                varchar(50)                    null,
   MB003                varchar(50)                    null,
   MB004                bit                            null,
   constraint PK_SYSMB primary key clustered (ID),
   constraint fk_sysmb foreign key(MB001) references SYSMA(ID)--����������ģ���������
);
--ɾ��ϵͳ������Ϣ
--drop table SYSMB
--ϵͳ�����ֵ�
create table SYSDA 
(
   ID                   int identity(1,1)              not null,
   DA001                varchar(100)                   null,
   DA002                varchar(50)                    null,
   DA003                varchar(200)                   null,
   constraint PK_SYSDA primary key clustered (ID)
);
--ɾ��ϵͳ�����ֵ�
--drop table SYSDA
--ϵͳ�ֵ���ϸ
create table SYSDB 
(
   ID                   int  identity(1,1)             not null,
   DB001                int                            null,
   DB002                varchar(50)                    null,
   DB003                varchar(50)                    null,
   DB004                varchar(200)                   null,
   constraint PK_SYSDB primary key clustered (ID),
   constraint fk_sysdb foreign key(DB001) references SYSDA(ID)--�����ֵ���ϸ�������ϵ
);
--ɾ��ϵͳ�����ֵ�
--drop table SYSDB
--��ɫ��Ϣ
create table USERA 
(
   ID                   int   identity(1,1)            not null,
   RA001                varchar(50)                    null,
   RA002                varchar(500)                   null,
   RA003                bit                            null,
   constraint PK_USERA primary key clustered (ID)
);
--ɾ����ɫ��Ϣ
--drop table USERA
--��ɫȨ�޷���
create table USERB 
(
   ID                   int identity(1,1)              not null,
   RB001                int                            null,
   RB002                int                            null,
   constraint PK_USERB primary key clustered (ID),
   constraint fk_rb001 foreign key(RB001) references USERA(ID),--������ɫȨ�޷����ɫ�����ϵ
   constraint fk_rb002 foreign key(RB002) references SYSMB(ID)--������ɫȨ�޷ֹ��������ϵ
);
--ɾ����ɫȨ�޷���
--drop table USERB

--�û���Ϣ��
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
--ɾ���û���Ϣ��
--drop table USEUA
--�û��˻���Ϣ
create table USEUB 
(
   ID                   bigint identity(1,1)          not null,
   UB001                bigint                         null,
   UB002                varchar(50)                    null,
   UB003                varchar(200)                   null,
   constraint PK_USEUB primary key clustered (ID),
   constraint fk_useub foreign key(UB001) references USEUA(ID)--�����˻���Ϣ�������ϵ
);
--ɾ���û��˻���Ϣ
--drop table USEUB
--�û���ɫ����
create table USERC 
(
   ID                   int identity(1,1)              not null,
   RC001                bigint                         null,
   RC002                int                            null,
   constraint PK_USERC primary key clustered (ID),
   constraint fk_rc001 foreign key(RC001) references USEUA(ID),--�����û���ɫ����-�û��������
   constraint fk_rc002 foreign key(RC002) references USERA(ID),--�����û���ɫ����-��ɫ�������
);
--ɾ���û���ɫ����
--drop table USERC
--ϵͳ��ǩ�����ࣩ
create table SYSTA 
(
   ID                   int identity(1,1)              not null,
   TA001                varchar(50)                    null,
   TA002                varchar(20)                    null,
   TA003                int                            null,
   TA004                varchar(200)                   null,
   constraint PK_SYSTA primary key clustered (ID)
);
--ɾ��ϵͳ��ǩ
--drop table SYSTA
--��ǩ����
create table SYSTB 
(
   ID                   int  identity(1,1)             not null,
   TB001                bigint                         null,
   TB002                int                            null,
   constraint PK_SYSTB primary key clustered (ID),
   constraint fk_tb001 foreign key(TB001) references USEUA(ID),--�����û���ǩ����-�û��������
   constraint fk_tb002 foreign key(TB002) references SYSTA(ID),--�����û���ǩ����-��ǩ�������
);
--ɾ����ǩ����
--drop table SYSTB
--ϵͳ��־
create table SYSLA 
(
   ID                   bigint identity(1,1)            not null,
   LA001                varchar(1000)                  null,
   LA002                varchar(200)                   null,
   LA003                varchar(200)                   null,
   LA004                bigint                         null,
   LA005                datetime                       null,
   constraint PK_SYSLA primary key clustered (ID),
   constraint fk_la004 foreign key(LA004) references USEUA(ID)--�����û�������־-�û��������
);
--ɾ��ϵͳ��־
--drop table SYSLA
go
