--Added new sub group

DECLARE @MyCursor CURSOR;
Declare @subjectGroupId int
BEGIN
    SET @MyCursor = CURSOR FOR
    SELECT SubjectGroupId from SubGroups
    GROUP BY SubGroups.SubjectGroupId     

    OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @subjectGroupId

    WHILE @@FETCH_STATUS = 0
    BEGIN
      
	  INSERT INTO SubGroups (Name, SubjectGroupId)
		VALUES ('third', @subjectGroupId);

      FETCH NEXT FROM @MyCursor 
      INTO @subjectGroupId 
    END; 

    CLOSE @MyCursor ;
    DEALLOCATE @MyCursor;
END;

--SubGroupsThird