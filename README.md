# 🏍️ Mottu API - Sistema de Gerenciamento de Pátio de Motos

---

## 📋 ENTREGA - 3º SPRINT

### 👥 Integrantes do Grupo:
- **Ana Carolina de Castro Gonçalves** - RM 554669
- **Luisa Danielle** - RM 555292
- **Michelle Marques Potenza** - RM 557702

---

### 🏗️ Justificativa da Arquitetura

#### 📊 Domínio Escolhido: Sistema de Pátio de Motos
A escolha do domínio de *gerenciamento de pátio de motos* se deu pela necessidade de implementar um sistema que gerencie a alocação de motos em um pátio, com o controle de vagas, status das motos e a associação entre funcionários, motos e clientes. Este cenário permite a aplicação de relações complexas entre as entidades e validações rigorosas de regras de negócios, o que torna o sistema desafiador e enriquecedor para o desenvolvimento de uma API robusta.

---

### 🧩 Arquitetura da API

A arquitetura foi desenhada de forma a garantir escalabilidade, manutenibilidade e clareza no fluxo de dados.

#### Camadas:
- **Controllers**: Responsáveis por gerenciar as requisições HTTP e interagir com a camada de serviços.
- **Services**: Contém a lógica de negócios e a comunicação com os repositórios.
- **Repositories**: Interface entre a aplicação e o banco de dados.
- **Database**: Persistência de dados utilizando *Entity Framework Core*.

#### 🎯 Principais Entidades:
- **Patio**: Representa os pátios de motos, gerencia as vagas e a localização.
- **Funcionario**: Funcionários que trabalham em pátios.
- **Moto**: Registra as motos, com seus status, modelos e setores.
- **Cliente**: Usuários que podem alugar motos.

#### 🔗 Relacionamentos:
- **Patio ↔ Funcionário**: Um pátio pode ter vários funcionários.
- **Patio ↔ Moto**: Um pátio pode abrigar várias motos.
- **Funcionario ↔ Moto**: Funcionários podem estar associados a motos.
- **Moto ↔ Cliente**: Motos podem ser alugadas por clientes, mas a associação é opcional.

---

### 🚀 Tecnologias Utilizadas
- **.NET 9.0**: Framework principal da API.
- **Entity Framework Core**: utilizado para interação com o banco de dados.
- **Oracle Database**: Banco de dados corporativo.
- **Swagger/OpenAPI**: Para documentação automática da API.
- **Swashbuckle**: Para gerar a documentação da API de forma automatizada.

---
## 🏃‍♂️ Execução da API

### Instruções para execução:

1. **Clonar o repositório**:
   ```bash
   git clone <url-do-repositorio>
   cd MottuApi3

2. **Restaurar as dependências**:
   ```bash
   dotnet restore


3. **Aplicar migrações do banco de dados**:
     ```bash
     dotnet ef migrations add InitialCreate --output-dir Data/Migrations
   dotnet ef database update


5. **Executar a aplicação**:
      ```bash

   dotnet run


🌐 Acesso à API:

Swagger UI: http://localhost:5147/
Base URL da API: http://localhost:5147


---
## --🎮 Exemplos de Uso dos Endpoints--

### 🔄 Sequência Correta de Operações:

#### 1. 🏢 Criar Pátio (Primeiro!)
POST /api/Patio

{
  "nomePatio": "Patio Itaim",
  "localizacao": "Brigadeiro Faria Lima, 920 - São Paulo/SP",
  "vagasTotais": 70
}

2. 👷 Criar Funcionário (Segundo!)
POST /api/Funcionario
```bash
  {
  "usuarioFuncionario": "LeoN10",
  "nome": "Leonardo Marques",
  "senha": "Pass123",
  "nomePatio": "Patio Itaim"
}

3. 🏍️ Criar Moto (Terceiro!)
POST /api/Moto

{
  "placa": "BBC-1234",
  "modelo": "MottuSport",
  "status": "Disponível",
  "setor": "Bom",
  "nomePatio": "Patio Itaim",
  "usuarioFuncionario": "LeoN10"
}

4. 👤 Criar Cliente (Opcional)
POST /api/Cliente

{
  "usuarioCliente": "mariaSantos",
  "nome": "Maria Santos",
  "senha": "mari456",
  "motoPlaca": "ABC-1234"
}

---

```
### 🚦 Validações Implementadas:
- **Formato de placa**: XXX-0000
- **Tamanho de senha**: Mínimo de 6 caracteres.
- **Vagas disponíveis**: Verificação antes de adicionar motos.
- **Integridade referencial**: Funcionário deve pertencer ao pátio.
- **Integridade referencial**: Moto deve ser alterada, adicionada ou removida apenas por Funcionário.


---

### 🎯 Funcionalidades Avançadas:
#### 🔢 Controle Automático de Vagas:
- **Moto Disponível ou em Manutenção**: Ocupa vaga.
- **Moto Alugada**: Libera vaga.
- **Atualização em Tempo Real**: As alterações no status da moto refletem nas vagas automaticamente.

---

### 📚 Documentação da API

📊 **Endpoints Disponíveis**:

| Método | Endpoint                       | Descrição                         |
|--------|---------------------------------|-----------------------------------|
| GET    | `/api/Patio`                    | Listar pátios com paginação       |
| POST   | `/api/Patio`                    | Criar novo pátio                  |
| GET    | `/api/Patio/{nome}/vagas`       | Consultar vagas disponíveis       |
| GET    | `/api/Funcionario`              | Listar funcionários               |
| POST   | `/api/Funcionario`              | Criar funcionário                 |
| GET    | `/api/Moto`                     | Listar motos com filtros          |
| POST   | `/api/Moto`                     | Criar moto                        |
| PUT    | `/api/Moto/{placa}`             | Atualizar moto                    |
| GET    | `/api/Cliente`                  | Listar clientes                   |
| POST   | `/api/Cliente`                  | Criar cliente                     |

---

## ✅ Testes Automáticos

### Executar os testes:
```bash
dotnet test

--- Executar testes com cobertura:
dotnet test --collect:"XPlat Code Coverage"
