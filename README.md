## Database

Create a SQL Server database in Azure.

Execute the following script to create the `ToDos` table:

```
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ToDos](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ToDos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```


## Azure

Create an Azure AD group.

```
az ad group create --display-name mattruma-045-sqldbs --mail-nickname mattruma-045-sqldbs
```

Add App Registration/Service Principal to the Azure AD Group

```
az ad group member add --group GROUP_ID --member-id MEMBER_ID_
```