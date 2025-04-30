CREATE TABLE [dbo].[MakeMarkA] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [OreMarkA]            FLOAT (53)    NOT NULL,
    [NikelMarkA]          FLOAT (53)    NOT NULL,
    [ChromeMarkA]         FLOAT (53)    NOT NULL,
    [MarganecMarkA]       FLOAT NOT NULL,
    [TimeFurnaceMarkA]    NVARCHAR (50) NOT NULL,
    [TimeConverterMarkA]  NVARCHAR (50) NOT NULL,
    [TimeRollingMachineA] NVARCHAR (50) NOT NULL, 
    [PriceA] FLOAT NOT NULL
	PRIMARY KEY CLUSTERED ([Id] ASC)
);

