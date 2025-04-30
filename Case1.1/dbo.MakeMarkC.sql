CREATE TABLE [dbo].[MakeMarkC] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [OreMarkC]            FLOAT (53)    NOT NULL,
    [NikelMarkC]          FLOAT (53)    NOT NULL,
    [ChromeMarkC]         FLOAT (53)    NOT NULL,
    [MarganecMarkC]       FLOAT NOT NULL,
    [TimeFurnaceMarkC]    NVARCHAR (50) NOT NULL,
    [TimeConverterMarkC]  NVARCHAR (50) NOT NULL,
    [TimeRollingMachineC] NVARCHAR (50) NOT NULL,
    [PriceC] FLOAT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

