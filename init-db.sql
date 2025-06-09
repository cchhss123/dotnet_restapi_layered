-- 先建立 資料庫[demo] 
CREATE DATABASE [demo];

USE [demo];

-- 建立 資料表 users
DROP TABLE IF EXISTS [dbo].[users];
CREATE TABLE [dbo].[users] (
	[id] bigint NOT NULL IDENTITY PRIMARY KEY,
	[name] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[email] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
);