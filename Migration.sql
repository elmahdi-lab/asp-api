CREATE TABLE IF NOT EXISTS clinics(
	id VARCHAR(50) NOT NULL PRIMARY KEY,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
)ENGINE=INNODB;

CREATE TRIGGER `clinics_insert_trigger` BEFORE INSERT ON `clinics`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
CREATE TRIGGER `clinics_update_trigger` BEFORE UPDATE ON `clinics`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
# ------------------------

CREATE TABLE IF NOT EXISTS patients(
	id varchar(101) GENERATED ALWAYS AS (concat(Firstname, ' ', Lastname)) STORED NOT NULL,
    Cinic_id VARCHAR(50) NOT NULL,
    Firstname VARCHAR(50) NOT NULL,
    Lastname VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    PRIMARY KEY (id),
    KEY `patients_ClinicID_fk` (`ClinicID`),
    CONSTRAINT `patients_ClinicID_fk`
		FOREIGN KEY (ClinicID) REFERENCES `clinics` (`id`) ON DELETE CASCADE
)ENGINE=INNODB;

CREATE TRIGGER `patients_insert_trigger` BEFORE INSERT ON `patients`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
CREATE TRIGGER `patients_update_trigger` BEFORE UPDATE ON `patients`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
-- 
# ------------------------

CREATE TABLE IF NOT EXISTS providers(
    id varchar(101) GENERATED ALWAYS AS (concat(Firstname, ' ', Lastname)) STORED NOT NULL,
    ClinicID VARCHAR(50) NOT NULL,
    Firstname VARCHAR(50) NOT NULL,
    Lastname VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    PRIMARY KEY (id),
    KEY `providers_ClinicID_fk` (`ClinicID`),
    CONSTRAINT `providers_ClinicID_fk` FOREIGN KEY (ClinicID)
        REFERENCES `clinics` (`id`) ON DELETE CASCADE
)ENGINE=INNODB;

CREATE TRIGGER `providers_insert_trigger` BEFORE INSERT ON `providers`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
CREATE TRIGGER `providers_update_trigger` BEFORE UPDATE ON `providers`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();

# ------------------------

CREATE TABLE IF NOT EXISTS availabilities(
    id int unsinged not null auto_increment,
    ProviderID varchar(101) not null,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    primary key (id),
    KEY `availabilities_ProviderID_fk` (`ProviderID`),
    CONSTRAINT `availabilities_ProviderID_fk` FOREIGN KEY (ProviderID)
        REFERENCES `providers` (`id`) ON DELETE CASCADE,
)ENGINE=INNODB;

CREATE TRIGGER `availabilities_insert_trigger` BEFORE INSERT ON `availabilities`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
CREATE TRIGGER `availabilities_update_trigger` BEFORE UPDATE ON `availabilities`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();

# ------------------------
CREATE TABLE IF NOT EXISTS appointments(
	id int unsigned NOT NULL auto_increment,
	PatientID varchar(101) NOT NULL,
    ProviderID varchar(101) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    PRIMARY KEY (id),
    KEY `appointments_PatientID_fk` (`PatientID`),
    KEY `appointments_ProviderID_fk` (`ProviderID`),
    CONSTRAINT `appointments_PatientID_fk` FOREIGN KEY (PatientID)
        REFERENCES `patients` (`id`) ON DELETE CASCADE,
    CONSTRAINT `appointments_ProviderID_fk` FOREIGN KEY (ProviderID)
        REFERENCES `providers` (`id`) ON DELETE CASCADE
)ENGINE=INNODB;

CREATE TRIGGER `appointments_insert_trigger` BEFORE INSERT ON `appointments`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
CREATE TRIGGER `appointments_update_trigger` BEFORE UPDATE ON `appointments`
	FOR EACH ROW SET NEW.CreatedAt = NOW(), NEW.UpdatedAt = NOW();
# ------------------------
