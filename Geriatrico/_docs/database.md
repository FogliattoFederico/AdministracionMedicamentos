-- =============================================
-- CREAR BASE DE DATOS
-- =============================================
CREATE DATABASE Geriatrico;
USE Geriatrico;

-- =============================================
-- TABLAS
-- =============================================
CREATE TABLE pacientes (
    id              INT PRIMARY KEY IDENTITY(1,1),
    nombre          VARCHAR(100) NOT NULL,
    apellido        VARCHAR(100) NOT NULL,
    dni             VARCHAR(20) UNIQUE NOT NULL,
    fecha_nac       DATE NOT NULL,
    habitacion      VARCHAR(10),
    obra_social     VARCHAR(100),
    nro_afiliado    VARCHAR(50),
    contacto_nombre VARCHAR(100),
    contacto_tel    VARCHAR(20),
    activo          BIT DEFAULT 1,
    created_at      DATETIME DEFAULT GETDATE()
);

CREATE TABLE medicamentos (
    id              INT PRIMARY KEY IDENTITY(1,1),
    nombre          VARCHAR(150) NOT NULL,
    nombre_generico VARCHAR(150),
    presentacion    VARCHAR(100),
    laboratorio     VARCHAR(100)
);

CREATE TABLE medicamentos_pacientes (
    id              INT PRIMARY KEY IDENTITY(1,1),
    paciente_id     INT NOT NULL,
    medicamento_id  INT NOT NULL,
    dosis           VARCHAR(100) NOT NULL,
    frecuencia      VARCHAR(100) NOT NULL,
    horarios        VARCHAR(200),
    via             VARCHAR(50),
    indicacion      TEXT,
    fecha_inicio    DATE NOT NULL,
    fecha_fin       DATE,
    activo          BIT DEFAULT 1,
    prescripto_por  VARCHAR(100),
    FOREIGN KEY (paciente_id) REFERENCES pacientes(id),
    FOREIGN KEY (medicamento_id) REFERENCES medicamentos(id)
);

CREATE TABLE administraciones (
    id                  INT PRIMARY KEY IDENTITY(1,1),
    medicamento_pac_id  INT NOT NULL,
    fecha               DATE NOT NULL,
    hora_programada     VARCHAR(10) NOT NULL,
    hora_administrada   VARCHAR(10),
    administrado_por    VARCHAR(100),
    tomado              BIT DEFAULT 0,
    observaciones       VARCHAR(300),
    created_at          DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (medicamento_pac_id) REFERENCES medicamentos_pacientes(id)
);

CREATE TABLE usuarios (
    id          INT PRIMARY KEY IDENTITY(1,1),
    nombre      VARCHAR(100) NOT NULL,
    email       VARCHAR(100) UNIQUE NOT NULL,
    password    VARCHAR(256) NOT NULL,
    activo      BIT DEFAULT 1,
    rol         VARCHAR(50) DEFAULT 'empleado',
    created_at  DATETIME DEFAULT GETDATE()
);

-- =============================================
-- USUARIOS
-- =============================================
INSERT INTO usuarios (nombre, email, password, rol)
VALUES ('Administrador', 'admin@geriatrico.com', 'admin123', 'admin');

-- =============================================
-- PACIENTES
-- =============================================
INSERT INTO pacientes (nombre, apellido, dni, fecha_nac, habitacion, obra_social, nro_afiliado, contacto_nombre, contacto_tel)
VALUES
('Juan',      'Pérez',      '11111111', '1938-03-12', '104', 'PAMI',  'P-011111', 'Diego Pérez',      '341-555-0001'),
('Ana',       'López',      '22222222', '1941-07-25', '105', 'OSDE',  'O-022222', 'Laura López',      '341-555-0002'),
('Carlos',    'Ramírez',    '33333333', '1936-11-08', '106', 'PAMI',  'P-033333', 'Marta Ramírez',    '341-555-0003'),
('Susana',    'Torres',     '44444444', '1943-02-14', '107', 'IOMA',  'I-044444', 'Pedro Torres',     '341-555-0004'),
('Ricardo',   'Sánchez',    '55555555', '1939-09-30', '108', 'PAMI',  'P-055555', 'Silvia Sánchez',   '341-555-0005'),
('Marta',     'Gómez',      '66666666', '1945-05-17', '109', 'OSDE',  'O-066666', 'Jorge Gómez',      '341-555-0006'),
('Luis',      'Díaz',       '77777777', '1937-08-22', '110', 'PAMI',  'P-077777', 'Rosa Díaz',        '341-555-0007'),
('Carmen',    'Flores',     '88888888', '1942-12-05', '111', 'SWISS', 'S-088888', 'Antonio Flores',   '341-555-0008'),
('Héctor',    'Morales',    '99999999', '1940-04-18', '112', 'PAMI',  'P-099999', 'Elena Morales',    '341-555-0009'),
('Patricia',  'Jiménez',    '10101010', '1944-06-29', '113', 'IOMA',  'I-010101', 'Roberto Jiménez',  '341-555-0010'),
('Alberto',   'Ruiz',       '11223344', '1935-01-15', '114', 'PAMI',  'P-011223', 'Graciela Ruiz',    '341-555-0011'),
('Norma',     'Herrera',    '22334455', '1946-10-03', '115', 'OSDE',  'O-022334', 'Gustavo Herrera',  '341-555-0012'),
('Osvaldo',   'Castro',     '33445566', '1938-07-11', '116', 'PAMI',  'P-033445', 'Beatriz Castro',   '341-555-0013'),
('Graciela',  'Romero',     '44556677', '1941-03-27', '117', 'IOMA',  'I-044556', 'Claudio Romero',   '341-555-0014'),
('Domingo',   'Vargas',     '55667788', '1943-09-09', '118', 'PAMI',  'P-055667', 'Nora Vargas',      '341-555-0015'),
('Beatriz',   'Medina',     '66778899', '1936-05-21', '119', 'SWISS', 'S-066778', 'Raúl Medina',      '341-555-0016'),
('Raúl',      'Aguilar',    '77889900', '1940-11-14', '120', 'PAMI',  'P-077889', 'Sandra Aguilar',   '341-555-0017'),
('Elsa',      'Ortiz',      '88990011', '1944-08-06', '121', 'OSDE',  'O-088990', 'Miguel Ortiz',     '341-555-0018'),
('Norberto',  'Molina',     '99001122', '1937-02-28', '122', 'PAMI',  'P-099001', 'Claudia Molina',   '341-555-0019'),
('Estela',    'Suárez',     '10203040', '1942-06-16', '123', 'IOMA',  'I-010203', 'Fernando Suárez',  '341-555-0020'),
('María',     'González',   '12345678', '1940-05-15', '101', 'PAMI',  'P-001234', 'Carlos González',  '341-555-1234'),
('Roberto',   'Fernández',  '23456789', '1935-11-20', '102', 'OSDE',  'O-005678', 'Ana Fernández',    '341-555-5678'),
('Elena',     'Martínez',   '34567890', '1942-03-08', '103', 'PAMI',  'P-009012', 'Luis Martínez',    '341-555-9012');

-- =============================================
-- MEDICAMENTOS
-- =============================================
INSERT INTO medicamentos (nombre, nombre_generico, presentacion, laboratorio)
VALUES
('Enalapril',       'Enalapril maleato',      'Comprimido 10mg',     'Roemmers'),
('Metformina',      'Metformina clorhidrato', 'Comprimido 500mg',    'Bagó'),
('Alprazolam',      'Alprazolam',             'Comprimido 0.25mg',   'Pfizer'),
('Omeprazol',       'Omeprazol',              'Cápsula 20mg',        'Gador'),
('Atenolol',        'Atenolol',               'Comprimido 50mg',     'Roemmers'),
('Amlodipina',      'Amlodipina besilato',    'Comprimido 5mg',      'Pfizer'),
('Losartán',        'Losartán potásico',      'Comprimido 50mg',     'Bagó'),
('Furosemida',      'Furosemida',             'Comprimido 40mg',     'Gador'),
('Espironolactona', 'Espironolactona',        'Comprimido 25mg',     'Roemmers'),
('Warfarina',       'Warfarina sódica',       'Comprimido 5mg',      'Roche'),
('Aspirina',        'Ácido acetilsalicílico', 'Comprimido 100mg',    'Bayer'),
('Simvastatina',    'Simvastatina',           'Comprimido 20mg',     'Gador'),
('Levotiroxina',    'Levotiroxina sódica',    'Comprimido 50mcg',    'Bagó'),
('Glibenclamida',   'Glibenclamida',          'Comprimido 5mg',      'Roemmers'),
('Insulina NPH',    'Insulina isofana',       'Inyectable 100UI/ml', 'Novo Nordisk'),
('Lorazepam',       'Lorazepam',              'Comprimido 1mg',      'Wyeth'),
('Haloperidol',     'Haloperidol',            'Comprimido 1mg',      'Gador'),
('Donepecilo',      'Donepecilo',             'Comprimido 5mg',      'Pfizer'),
('Calcio + Vit D',  'Carbonato de calcio',    'Comprimido 500mg',    'Bagó');

-- =============================================
-- MEDICAMENTOS POR PACIENTE
-- =============================================

-- Juan Pérez (dni 11111111) - Hipertensión, Colesterol
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '11111111' AND m.nombre = 'Enalapril';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Colesterol', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '11111111' AND m.nombre = 'Simvastatina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Prevención cardiovascular', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '11111111' AND m.nombre = 'Aspirina';

-- Ana López (dni 22222222) - Diabetes, Hipertensión, Ansiedad
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 12 horas', '08:00, 20:00', 'Oral', 'Diabetes tipo 2', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '22222222' AND m.nombre = 'Metformina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '22222222' AND m.nombre = 'Losartán';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '22:00', 'Oral', 'Ansiedad', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '22222222' AND m.nombre = 'Lorazepam';

-- Carlos Ramírez (dni 33333333) - Hipertensión, Anticoagulación
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '33333333' AND m.nombre = 'Atenolol';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Anticoagulación', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '33333333' AND m.nombre = 'Warfarina';

-- Susana Torres (dni 44444444) - Hipotiroidismo, Osteoporosis
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipotiroidismo', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '44444444' AND m.nombre = 'Levotiroxina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Osteoporosis', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '44444444' AND m.nombre = 'Calcio + Vit D';

-- Ricardo Sánchez (dni 55555555) - Diabetes, Hipertensión
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Diabetes tipo 2', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '55555555' AND m.nombre = 'Glibenclamida';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '55555555' AND m.nombre = 'Amlodipina';

-- Marta Gómez (dni 66666666) - Insuficiencia cardíaca
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Insuficiencia cardíaca', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '66666666' AND m.nombre = 'Furosemida';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Insuficiencia cardíaca', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '66666666' AND m.nombre = 'Espironolactona';

-- Luis Díaz (dni 77777777) - Alzheimer, Trastorno conductual
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Alzheimer', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '77777777' AND m.nombre = 'Donepecilo';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '22:00', 'Oral', 'Trastorno conductual', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '77777777' AND m.nombre = 'Haloperidol';

-- Carmen Flores (dni 88888888) - Hipertensión, Colesterol
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '88888888' AND m.nombre = 'Enalapril';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Colesterol', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '88888888' AND m.nombre = 'Simvastatina';

-- Héctor Morales (dni 99999999) - Hipertensión, Prevención cardiovascular
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '99999999' AND m.nombre = 'Losartán';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Prevención cardiovascular', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '99999999' AND m.nombre = 'Aspirina';

-- Patricia Jiménez (dni 10101010) - Diabetes, Osteoporosis
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 12 horas', '08:00, 20:00', 'Oral', 'Diabetes tipo 2', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '10101010' AND m.nombre = 'Metformina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Osteoporosis', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '10101010' AND m.nombre = 'Calcio + Vit D';

-- Alberto Ruiz (dni 11223344) - Hipertensión
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '11223344' AND m.nombre = 'Atenolol';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Gastritis', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '11223344' AND m.nombre = 'Omeprazol';

-- Norma Herrera (dni 22334455) - Hipotiroidismo
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipotiroidismo', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '22334455' AND m.nombre = 'Levotiroxina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Colesterol', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '22334455' AND m.nombre = 'Simvastatina';

-- Osvaldo Castro (dni 33445566) - Insuficiencia cardíaca
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Insuficiencia cardíaca', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '33445566' AND m.nombre = 'Furosemida';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '33445566' AND m.nombre = 'Amlodipina';

-- Graciela Romero (dni 44556677) - Ansiedad, Osteoporosis
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '22:00', 'Oral', 'Ansiedad', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '44556677' AND m.nombre = 'Lorazepam';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Osteoporosis', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '44556677' AND m.nombre = 'Calcio + Vit D';

-- Domingo Vargas (dni 55667788) - Hipertensión
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '55667788' AND m.nombre = 'Amlodipina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Prevención cardiovascular', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '55667788' AND m.nombre = 'Aspirina';

-- Beatriz Medina (dni 66778899) - Anticoagulación
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Anticoagulación', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '66778899' AND m.nombre = 'Warfarina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dra. Martínez' FROM pacientes p, medicamentos m WHERE p.dni = '66778899' AND m.nombre = 'Atenolol';

-- Raúl Aguilar (dni 77889900) - Alzheimer
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Alzheimer', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '77889900' AND m.nombre = 'Donepecilo';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '22:00', 'Oral', 'Trastorno conductual', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '77889900' AND m.nombre = 'Haloperidol';

-- Elsa Ortiz (dni 88990011) - Colesterol, Hipertensión
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Colesterol', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '88990011' AND m.nombre = 'Simvastatina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '88990011' AND m.nombre = 'Losartán';

-- Norberto Molina (dni 99001122) - Hipertensión, Gastritis
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '99001122' AND m.nombre = 'Enalapril';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 cápsula', 'Cada 24 horas', '08:00', 'Oral', 'Gastritis', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '99001122' AND m.nombre = 'Omeprazol';

-- Estela Suárez (dni 10203040) - Osteoporosis, Ansiedad
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Osteoporosis', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '10203040' AND m.nombre = 'Calcio + Vit D';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '22:00', 'Oral', 'Ansiedad', '2024-01-01', 'Dr. García' FROM pacientes p, medicamentos m WHERE p.dni = '10203040' AND m.nombre = 'Alprazolam';

-- María González (dni 12345678) - Hipertensión, Gastritis
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '12345678' AND m.nombre = 'Enalapril';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 cápsula', 'Cada 24 horas', '08:00', 'Oral', 'Gastritis', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '12345678' AND m.nombre = 'Omeprazol';

-- Roberto Fernández (dni 23456789) - Diabetes
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 12 horas', '08:00, 20:00', 'Oral', 'Diabetes tipo 2', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '23456789' AND m.nombre = 'Metformina';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Colesterol', '2024-01-01', 'Dra. López' FROM pacientes p, medicamentos m WHERE p.dni = '23456789' AND m.nombre = 'Simvastatina';

-- Elena Martínez (dni 34567890) - Ansiedad, Hipertensión
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '22:00', 'Oral', 'Ansiedad', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '34567890' AND m.nombre = 'Alprazolam';
INSERT INTO medicamentos_pacientes (paciente_id, medicamento_id, dosis, frecuencia, horarios, via, indicacion, fecha_inicio, prescripto_por)
SELECT p.id, m.id, '1 comprimido', 'Cada 24 horas', '08:00', 'Oral', 'Hipertensión', '2024-01-01', 'Dr. Pérez' FROM pacientes p, medicamentos m WHERE p.dni = '34567890' AND m.nombre = 'Losartán';
