-- Criando tabela SituacaoCliente
CREATE TABLE SituacaoCliente (
    id INT IDENTITY(1,1) PRIMARY KEY,
    Descricao VARCHAR(50) NOT NULL

);
GO

INSERT INTO SituacaoCliente (Descricao) VALUES ('Ativo'), ('Inativo'), ('Bloqueado');

-- Criando Tabela Cliente
CREATE TABLE Cliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(150) NOT NULL, 
    Cpf VARCHAR(11) NOT NULL UNIQUE,
    DataNascimento DATE NOT NULL,
    Sexo CHAR(1) NOT NULL,
    SituacaoClienteId INT NOT NULL,
    -- fk que aponta para SituacaoCliente
    CONSTRAINT FK_Cliente_Situacao FOREIGN KEY (SituacaoClienteId) REFERENCES SituacaoCliente(Id)
);
GO

-- Procedure InserirCliente
CREATE PROCEDURE sp_InserirCliente
    @Nome VARCHAR(150),
    @Cpf VARCHAR(11),
    @DataNascimento DATE,
    @Sexo CHAR(1),
    @SituacaoClienteId INT
AS
BEGIN
-- try catch no banco para tratar erros de inserção
BEGIN TRY
        INSERT INTO Cliente (Nome, Cpf, DataNascimento, Sexo, SituacaoClienteId)
        VALUES (@Nome, @Cpf, @DataNascimento, @Sexo, @SituacaoClienteId);
    END TRY
    BEGIN CATCH
      
        THROW;
    END CATCH
END
GO

-- Procedure AtualizarCliente
CREATE PROCEDURE sp_AtualizarCliente
    @Id INT,
    @Nome VARCHAR(150),
    @Cpf VARCHAR(11),
    @DataNascimento DATE,
    @Sexo CHAR(1),
    @SituacaoClienteId INT
AS
BEGIN

    BEGIN TRY
        UPDATE Cliente
        SET Nome = @Nome,
            Cpf = @Cpf,
            DataNascimento = @DataNascimento,
            Sexo = @Sexo,
            SituacaoClienteId = @SituacaoClienteId
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- Procedure ExcluirCliente

CREATE PROCEDURE sp_ExcluirCliente
    @Id INT
AS
BEGIN
    BEGIN TRY
        DELETE FROM Cliente WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- Procedure ListarClientes
CREATE PROCEDURE sp_ListarClientes
AS
BEGIN
    SELECT C.*, S.Descricao AS SituacaoDescricao
    FROM Cliente C
    INNER JOIN SituacaoCliente S ON C.SituacaoClienteId = S.Id;
END
GO
