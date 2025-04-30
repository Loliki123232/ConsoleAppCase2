-- Таблица для марки B
CREATE TABLE [dbo].[MakeMarkB]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OreMarkB] INT NOT NULL, 
    [NikelMarkB] INT NOT NULL, 
    [ChromeMarkB] INT NOT NULL, 
    [MarganecMarkB] NVARCHAR(50) NOT NULL, 
    [TimeFurnaceMarkB] NVARCHAR(50) NOT NULL, 
    [TimeConverterMarkB] NVARCHAR(50) NOT NULL, 
    [TimeRollingMachineB] NVARCHAR(50) NOT NULL
)

-- Таблица для марки C
CREATE TABLE [dbo].[MakeMarkC]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OreMarkC] INT NOT NULL, 
    [NikelMarkC] INT NOT NULL, 
    [ChromeMarkC] INT NOT NULL, 
    [MarganecMarkC] NVARCHAR(50) NOT NULL, 
    [TimeFurnaceMarkC] NVARCHAR(50) NOT NULL, 
    [TimeConverterMarkC] NVARCHAR(50) NOT NULL, 
    [TimeRollingMachineC] NVARCHAR(50) NOT NULL
)