## Teste Técnico - Fácil Assist

### Diagrama Entidade-Relacionamento (DER)

![Diagrama de Entidade-Relacionamento das tabelas Cliente e SituacaoCliente](assets/Untitled-2026-05-29-2134.png)

### Problema encontrado na instalação do SQL Server

Durante a instalação do SQL Server 2022 apareceu o erro `0x851A001A`, relacionado ao SSD/NVMe e ao tamanho de setor informado pelo Windows. Após aplicar o ajuste recomendado, reiniciei a máquina, removi a instalação quebrada e instalei novamente o SQL Server 2022 Developer com o recurso `Serviços de Mecanismo de Banco de Dados`.

### Banco de dados

Foi criado o banco `FacilAssistDB` no SQL Server. O script principal está em:

```text
Database/01_Tabelas_e_Procedures.sql
```

Ele cria as tabelas `Cliente` e `SituacaoCliente`, além das procedures de inserir, alterar, excluir e listar clientes.
