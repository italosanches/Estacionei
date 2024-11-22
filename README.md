# Estacionei 🚗
API desenvolvida para controlar as entradas e saídas de veículos em um estacionamento. A aplicação permite o cadastro de clientes, veículos com seu respectivo tipo, configuração de tarifas por hora com base no tipo de veículo, além de registrar entradas e saídas.

## Como Utilizar?

1. **Instalar o Docker**.
2. **Clonar o repositorio.**
3. **Abra o terminal e va ate a pasta raiz da solução com o comando CD, exemplo C:\Users\Usuario\source\repos\Estacionei**.
4. **Rode o seguinte comando no terminal: docker-compose up --build**'Aguarde o docker fazer o trabalho. =)'
5. **Acesse o Swagger via http://localhost:8080/swagger/index.html**:
6. **Utilize o seguinte login para a autenticar no EndPoint de Login**
   ``` Json
      {
        "email": "admin@default.com",
        "password": "P@ssw0rd123!"
      }

8. **Desfrute da API**
   
# Endpoints

### **Autenticação**  
Serviço para criar e remover permissões, criar e remover usuários, e listar usuários.

---

### **Clientes** 🙎  
Todos os endpoints voltados ao cadastro ou atualização de clientes exigem que o telefone seja obrigatório e válido no seguinte formato: **1799999999**.

---

### **Veículos** 🚗  
Cadastro, listagem, remoção e atualização de veículos. As placas são validadas para evitar duplicidades.  
Um cliente pode possuir **N veículos**.

**Tipos de veículos permitidos:**  
1. Carro  
2. Moto  
3. Camionete  

---

### **Configuração de Valor por Hora** 🕛  
Cadastro do valor cobrado por hora para cada tipo de veículo. É permitido apenas um cadastro para cada tipo de veículo.  

**Tipos de veículos permitidos:**  
1. Carro  
2. Moto  
3. Camionete  

---

### **Entradas** 🎟️  
Entrada no estacionamento. Deve ser passado o **ID do veículo**.  

- Validações realizadas:  
  - A data inserida deve ser válida e pode ser uma data futura ou passada.  
  - É verificado se há uma entrada em aberto para o veículo. **Não é permitido mais de uma entrada em aberto para o mesmo veículo.**

---

### **Saídas** 🎟️  
Saída do estacionamento.  

- Cálculo realizado:  
  - O tempo que o veículo permaneceu no estacionamento é multiplicado pelo valor configurado por hora do tipo respectivo do veículo.  

---
