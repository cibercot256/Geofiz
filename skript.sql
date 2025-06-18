SET LANGUAGE English;

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(255) NULL
);

-- ������� ��� �������� ���������� � ����������
CREATE TABLE Geophysicists (
    GeophysicistID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Organization NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(255) NULL
);

-- ������� ��� �������� ���������� �� ����������
CREATE TABLE Analysts (
    AnalystID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Organization NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(255) NULL
);

-- ������� ��� �������� ���������� �� ���������������
CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(255) NULL
);

-- ������� ��� �������� ���������� � ���������
CREATE TABLE Wells (
    WellID INT PRIMARY KEY IDENTITY(1,1),
    UniqueCode NVARCHAR(50) NOT NULL,
    Coordinates NVARCHAR(100) NOT NULL,
    Area NVARCHAR(100) NOT NULL
);

-- ������� ��� �������� ����� ��������
CREATE TABLE LoggingTypes (
    LoggingTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50) NOT NULL
);

-- ������� ��� �������� ���������
CREATE TABLE Measurements (
    MeasurementID INT PRIMARY KEY IDENTITY(1,1),
    WellID INT NOT NULL,
    LoggingTypeID INT NOT NULL,
    Depth DECIMAL(10, 2) NOT NULL,
    MeasurementValue DECIMAL(10, 2) NOT NULL,
    MeasurementDateTime DATETIME NOT NULL,
    Operator NVARCHAR(100) NOT NULL,
    Contract NVARCHAR(100) NULL,
    Comment NVARCHAR(255) NULL,
    FOREIGN KEY (WellID) REFERENCES Wells(WellID),
    FOREIGN KEY (LoggingTypeID) REFERENCES LoggingTypes(LoggingTypeID)
);

-- ������� ������ � ����� ��������
INSERT INTO LoggingTypes (TypeName) VALUES
('�������������'),
('�������������'),
('������������'),
('����������');


-- ������ ������� ������ � ���������
INSERT INTO Customers (Name, ContactInfo) VALUES
('�������� � ������������ ���������������� "�������"', 'example@gazprom.ru');

-- ������ ������� ������ � ���������
INSERT INTO Geophysicists (Name, Organization, ContactInfo) VALUES
('������ �.�.', '������������� ��������', 'ivanov@geophy.com');

-- ������ ������� ������ � ���������
INSERT INTO Analysts (Name, Organization, ContactInfo) VALUES
('������ �.�.', '������������� �����������', 'petrov@analyzlab.com');
ALTER TABLE Measurements
ADD
    Resistance DECIMAL(10,2) NULL,
    Radioactivity DECIMAL(10,2) NULL,
    SoundSpeed DECIMAL(10,2) NULL,
    Neutrons DECIMAL(10,2) NULL,
	LoggingPoint NVARCHAR(50) NULL,
	Coordinates NVARCHAR(100);
ALTER TABLE Wells ADD Profile NVARCHAR(100);
ALTER TABLE Wells ADD ProfileDescription NVARCHAR(255);
ALTER TABLE Wells ADD LoggingPoint NVARCHAR(50);

	INSERT INTO Wells (UniqueCode, Coordinates, Area, LoggingPoint) VALUES
('WELL-001', '0,0;100,0;100,100;0,100', '22', '60.5,75.3'),
('WELL-002', '0,0;100,0;100,100;0,100', '50', '12;12'),
('WELL-003', '10,0;100,0;100,100;0,100', '1500', '12;12'),
('WELL-004', '60,20;50,0;100,100;0,100', '2630', '12;12'),
('WELL-005', '83,11;100,0;100,100;0,100', '13369', '12;12');
-- ������������� ������� � �������������
INSERT INTO Measurements (WellID, LoggingTypeID, Depth, MeasurementValue, MeasurementDateTime, Operator, Resistance,LoggingPoint )
VALUES
(1, 1, 150.5, 70.2, '2024-06-10 10:30:00', '�������', 1205, '60.5,75.3'),
(1, 1, 200.0, 68.4, '2024-06-12 11:15:00', '�������', 1102, '60.5,75.3'),
(2, 1, 300.5, 75.1, '2024-06-14 09:45:00', '�������', 1303, '60.5,75.3'),
(2, 1, 450.7, 66.9, '2024-06-15 08:10:00', '�������', 904, '60.5,75.3'),
(3, 1, 100.3, 80.2, '2024-06-16 12:30:00', '�������', 1157, '60.5,75.3'),
(3, 1, 120.9, 81.7, '2024-06-17 14:00:00', '�������', 1088, '60.5,75.3'),
(4, 1, 210.4, 77.0, '2024-06-18 13:20:00', '�������', 1221, '60.5,75.3'),
(4, 1, 260.6, 73.5, '2024-06-19 16:45:00', '�������', 973, '60.5,75.3'),
(5, 1, 310.7, 85.1, '2024-06-20 09:00:00', '�������', 1005, '60.5,75.3'),
(5, 1, 360.2, 88.9, '2024-06-21 15:10:00', '�������', 1054, '60.5,75.3');

-- ������������� ������� � ���������������
INSERT INTO Measurements (WellID, LoggingTypeID, Depth, MeasurementValue, MeasurementDateTime, Operator, Radioactivity)
VALUES
(1, 2, 180.0, 50.1, '2024-06-11 10:00:00', '�������', 220.5),
(2, 2, 220.6, 52.7, '2024-06-13 11:30:00', '�������', 245.3),
(3, 2, 310.4, 49.0, '2024-06-14 14:20:00', '�������', 265.8),
(4, 2, 280.9, 51.4, '2024-06-16 13:10:00', '�������', 240.2),
(5, 2, 150.1, 55.9, '2024-06-18 12:00:00', '�������', 290.6),
(1, 2, 320.8, 47.7, '2024-06-19 10:45:00', '�������', 275.1),
(2, 2, 410.5, 56.2, '2024-06-20 09:30:00', '�������', 265.0),
(3, 2, 370.7, 58.4, '2024-06-21 08:15:00', '�������', 260.9),
(4, 2, 430.9, 53.5, '2024-06-22 14:00:00', '�������', 280.0),
(5, 2, 390.2, 57.3, '2024-06-23 11:20:00', '�������', 295.4);

-- ������������ ������� � �������� �����
INSERT INTO Measurements (WellID, LoggingTypeID, Depth, MeasurementValue, MeasurementDateTime, Operator, SoundSpeed)
VALUES
(1, 3, 140.3, 60.2, '2024-06-09 09:00:00', '�������', 1650.5),
(2, 3, 160.6, 62.5, '2024-06-10 10:20:00', '�������', 1720.3),
(3, 3, 190.8, 64.0, '2024-06-11 12:00:00', '�������', 1555.1),
(4, 3, 210.4, 63.7, '2024-06-12 14:10:00', '�������', 1670.6),
(5, 3, 240.7, 59.3, '2024-06-13 15:45:00', '�������', 1780.9),
(1, 3, 270.1, 65.1, '2024-06-14 08:30:00', '�������', 1820.0),
(2, 3, 300.0, 66.9, '2024-06-15 11:00:00', '�������', 1605.2),
(3, 3, 330.5, 68.4, '2024-06-16 12:45:00', '�������', 1760.3),
(4, 3, 350.2, 69.5, '2024-06-17 09:50:00', '�������', 1705.7),
(5, 3, 380.9, 70.0, '2024-06-18 16:00:00', '�������', 1800.4);

-- ���������� ������� � ��������
INSERT INTO Measurements (WellID, LoggingTypeID, Depth, MeasurementValue, MeasurementDateTime, Operator, Neutrons)
VALUES
(1, 4, 160.1, 40.3, '2024-06-09 08:00:00', '�������', 510.2),
(2, 4, 200.4, 43.7, '2024-06-10 09:30:00', '�������', 475.6),
(3, 4, 250.6, 45.0, '2024-06-11 11:15:00', '�������', 490.1),
(4, 4, 280.3, 46.2, '2024-06-12 13:10:00', '�������', 520.8),
(5, 4, 300.0, 47.5, '2024-06-13 14:25:00', '�������', 550.3),
(1, 4, 320.8, 49.1, '2024-06-14 15:30:00', '�������', 600.0),
(2, 4, 340.6, 48.7, '2024-06-15 10:45:00', '�������', 565.7),
(3, 4, 360.9, 50.4, '2024-06-16 12:10:00', '�������', 530.2),
(4, 4, 380.1, 51.3, '2024-06-17 16:40:00', '�������', 495.0),
(5, 4, 400.7, 52.0, '2024-06-18 09:15:00', '�������', 610.4);

