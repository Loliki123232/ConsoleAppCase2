CREATE TABLE [dbo].[MakeMarkB] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [OreMarkB]            FLOAT (53)    NOT NULL,
    [NikelMarkB]          FLOAT (53)    NOT NULL,
    [ChromeMarkB]         FLOAT (53)    NOT NULL,
    [MarganecMarkB]       FLOAT NOT NULL,
    [TimeFurnaceMarkB]    NVARCHAR (50) NOT NULL,
    [TimeConverterMarkB]  NVARCHAR (50) NOT NULL,
    [TimeRollingMachineB] NVARCHAR (50) NOT NULL,
    [PriceB] FLOAT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

