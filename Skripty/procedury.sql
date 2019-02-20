CREATE PROCEDURE VymenitMotor (
	@p_id int,
	@p_motor int
) AS
BEGIN
	DECLARE @v_pocet int;

	BEGIN TRANSACTION
	BEGIN TRY
		UPDATE dbo.Jezdci SET Motory_Seriove_cislo=@p_motor WHERE ID = @p_id;
		
		SELECT @v_pocet=Pocet_pouziti FROM Motory WHERE Seriove_cislo=@p_motor;

		SET @v_pocet = @v_pocet + 1;

		UPDATE Motory SET Pocet_pouziti = @v_pocet WHERE Seriove_cislo=@p_motor;
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
	END CATCH
END

CREATE PROCEDURE VlozitUzivatele (
	@login varchar(40),
	@heslo varchar(60),
	@typ varchar(10),
	@info1_string varchar(40),
	@info2_string varchar(20),
	@info1_int int,
	@info2_int int,
	@info1_double decimal,
	@info1_datum date
) AS
BEGIN
	DECLARE @id_next int;
	DECLARE @id_next_typ int;

	BEGIN TRANSACTION
	BEGIN TRY
		IF (@typ = 'Team') BEGIN
			SELECT @id_next=MAX(ID) FROM Uzivatel;
			SET @id_next = @id_next + 1;
			SELECT @id_next_typ=MAX(ID) FROM Tymy;
			SET @id_next_typ = @id_next_typ + 1;
			INSERT INTO Uzivatel VALUES (@id_next, @login, @heslo, @typ);
			INSERT INTO Tymy VALUES (@id_next_typ, @info1_string, @info2_string, @info1_int, @info2_int, @id_next);
		END
		IF (@typ = 'GP') BEGIN
			SELECT @id_next=MAX(ID) FROM Uzivatel;
			SET @id_next = @id_next + 1;
			SELECT @id_next_typ=MAX(ID) FROM GP;
			SET @id_next_typ = @id_next_typ + 1;
			INSERT INTO Uzivatel VALUES (@id_next, @login, @heslo, @typ);
			INSERT INTO GP VALUES (@id_next_typ, @info1_string, @info1_datum, @info1_double, @info1_int, @info2_int, @id_next);
		END
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
	END CATCH
END