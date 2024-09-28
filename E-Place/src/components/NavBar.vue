<template>
  <div>
    <nav class="navbar navbar-expand-lg fixed-top">
      <div class="container-fluid">
        <div class="navbar-brand">
          <h1>
            <a href="/" class="navbar-brand">
            <img id="logoNav" src="../assets/img/logo.png" alt="E-Place">
            <strong class="logo-text">E-place</strong>
            </a>
          </h1>
        </div>
        <div class="navbar-collapse justify-content-end">
          <template v-if="!isUserArea">
            <button type="button" class="btn custom-btn" @click="openLoginModal">
              <strong>Login</strong>
            </button>
          </template>
          <template v-else>
            <button type="button" class="btn custom-btn" data-bs-toggle="modal" data-bs-target="#escolhaEcommerceModal">
              <strong>Cadastrar Produto</strong>
            </button>
          </template>
        </div>
      </div>
    </nav>

    <!-- Modal de login -->
    <div class="modal fade" id="loginModal" tabindex="-1" aria-labelledby="loginModalLabel" aria-hidden="true" ref="loginModal">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-body">
            <div class="container" id="container">
              <div class="form-container sign-up-container">
                <form @submit.prevent="register">
                  <h1 class="login">Criar Conta</h1>
                  <div class="social-container">
                    <a href="#" class="social"><i class="fab fa-facebook-f"></i></a>
                    <a href="#" class="social"><i class="fab fa-google-plus-g"></i></a>
                    <a href="#" class="social"><i class="fab fa-linkedin-in"></i></a>
                  </div>
                  <span>ou use seu email para registrar</span>
                  <input type="text" placeholder="Nome" v-model="registerName" />
                  <input type="email" placeholder="Email" v-model="registerEmail" />
                  <input type="password" placeholder="Senha" v-model="registerPassword" />
                  <p v-if="registerError" class="error-message">{{ registerError }}</p>
                  <button type="submit">Registrar</button>
                </form>
              </div>
              <div class="form-container sign-in-container">
                <form @submit.prevent="login">
                  <h1 class="login">Entrar</h1>
                  <div class="social-container">
                    <a href="#" class="social"><i class="fab fa-facebook-f"></i></a>
                    <a href="#" class="social"><i class="fab fa-google-plus-g"></i></a>
                    <a href="#" class="social"><i class="fab fa-linkedin-in"></i></a>
                  </div>
                  <span>ou use sua conta</span>
                  <input type="email" placeholder="Email" v-model="loginEmail" />
                  <input type="password" placeholder="Senha" v-model="loginPassword" />
                  <a href="#">Esqueceu sua senha?</a>
                  <p v-if="loginError" class="error-message">{{ loginError }}</p>
                  <button type="submit">Entrar</button>
                </form>
              </div>
              <div class="overlay-container">
                <div class="overlay">
                  <div class="overlay-panel overlay-left">
                    <h1>Bem-vindo de Volta!</h1>
                    <p></p>
                    <button class="ghost" id="signIn">Entrar</button>
                  </div>
                  <div class="overlay-panel overlay-right">
                    <h1>Olá, Amigo!</h1>
                    <p></p>
                    <button class="ghost" id="signUp">Registrar</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>;
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { Modal } from 'bootstrap';

export default {
  setup() {
    const router = useRouter();
    const isUserArea = ref(false);
    const loginEmail = ref('');
    const loginPassword = ref('');
    const registerName = ref('');
    const registerEmail = ref('');
    const registerPassword = ref('');
    const registeredUser = ref(null);
    const loginModal = ref(null);
    let modal = null;

    const loginError = ref('');
    const registerError = ref('');

    onMounted(() => {
      if (loginModal.value) {
        modal = new Modal(loginModal.value);
      }

      // Lógica para alternar entre os painéis de login e registro
      const signUpButton = document.getElementById('signUp');
      const signInButton = document.getElementById('signIn');
      const container = document.getElementById('container');

      if (signUpButton && signInButton && container) {
        signUpButton.addEventListener('click', () => {
          container.classList.add("right-panel-active");
        });

        signInButton.addEventListener('click', () => {
          container.classList.remove("right-panel-active");
        });
      }
    });

    const openLoginModal = () => {
      if (!isUserArea.value && modal) {
        modal.show();
      }
    };

    const login = () => {
  loginError.value = '';

  if (!loginEmail.value || !loginPassword.value) {
    loginError.value = 'Por favor, preencha todos os campos.';
    return;
  }

  if (registeredUser.value && loginEmail.value === registeredUser.value.email && loginPassword.value === registeredUser.value.password) {
    localStorage.setItem('user', JSON.stringify(registeredUser.value)); // Armazena informações do usuário no localStorage
    if (modal) {
      modal.hide();

      setTimeout(() => {
        router.push('/perfil');
        isUserArea.value = true; 
      }, 100);
    }
  } else {
    loginError.value = 'Usuário ou senha incorretos. Tente novamente.';
  }
};

    const register = () => {
      registerError.value = '';

      if (!registerName.value || !registerEmail.value || !registerPassword.value) {
        registerError.value = 'Por favor, preencha todos os campos.';
        return;
      }

      if (!registerEmail.value.includes('@')) {
        registerError.value = 'Por favor, insira um email válido.';
        return;
      }

      registeredUser.value = {
        name: registerName.value,
        email: registerEmail.value,
        password: registerPassword.value
      };
      alert('Registro realizado com sucesso!');
      loginEmail.value = registerEmail.value;
      loginPassword.value = registerPassword.value;
      login();  // Realiza o login automaticamente após o registro
    };

    return {
      isUserArea,
      loginEmail,
      loginPassword,
      registerName,
      registerEmail,
      registerPassword,
      openLoginModal,
      login,
      register,
      loginModal,
      loginError,
      registerError
    };
  }
};
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css?family=Montserrat:400,800');

* {
  box-sizing: border-box;
}

body {
  background: #f6f5f7;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  font-family: 'Montserrat', sans-serif;
  height: 100vh;
  margin: -20px 0 50px;
}

h1 {
  font-weight: bold;
  margin: 0;
}

h2 {
  text-align: center;
}

p {
  font-size: 14px;
  font-weight: 100;
  line-height: 20px;
  letter-spacing: 0.5px;
  margin: 20px 0 30px;
}

span {
  font-size: 12px;
}

a {
  color: #333;
  font-size: 14px;
  text-decoration: none;
  margin: 15px 0;
}

button {
  border-radius: 20px;
  border: 1px solid #FFBD59;
  background-color: #FFBD59;
  color: #FFFFFF;
  font-size: 12px;
  font-weight: bold;
  padding: 12px 45px;
  letter-spacing: 1px;
  text-transform: uppercase;
  transition: transform 80ms ease-in;
}

button:active {
  transform: scale(0.95);
}

button:focus {
  outline: none;
}

button.ghost {
  background-color: transparent;
  border-color: #FFFFFF;
}

form {
  background-color: #FFFFFF;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  padding: 0 50px;
  height: 100%;
  text-align: center;
}

input {
  background-color: #eee;
  border: none;
  padding: 12px 15px;
  margin: 8px 0;
  width: 100%;
}

.container {
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 14px 28px rgba(0, 0, 0, 0.25),
              0 10px 10px rgba(0, 0, 0, 0.22);
  position: relative;
  overflow: hidden;
  width: 768px;
  max-width: 95%;
  min-height: 480px;
}

.form-container {
  position: absolute;
  top: 0;
  height: 100%;
  transition: all 0.6s ease-in-out;
}

.sign-in-container {
  left: 0;
  width: 50%;
  z-index: 2;
}

.container.right-panel-active .sign-in-container {
  transform: translateX(100%);
}

.sign-up-container {
  left: 0;
  width:  50%;
  opacity: 0;
  z-index: 1;
}

.container.right-panel-active .sign-up-container {
  transform: translateX(100%);
  opacity: 1;
  z-index: 5;
  animation: show 0.6s;
}

@keyframes show {
  0%, 49.99% {
    opacity: 0;
    z-index: 1;
  }

  50%, 100% {
    opacity: 1;
    z-index: 5;
  }
}

.overlay-container {
  position: absolute;
  top: 0;
  left: 50%;
  width: 50%;
  height: 100%;
  overflow: hidden;
  transition: transform 0.6s ease-in-out;
  z-index: 100;
}

.container.right-panel-active .overlay-container {
  transform: translateX(-100%);
}

.overlay {
  background: #FFBD59;
  background: -webkit-linear-gradient(to right, #FFBD59, #FFBD59);
  background: linear-gradient(to right, #FFBD59, #FFBD59);
  background-repeat: no-repeat;
  background-size: cover;
  background-position: 0 0;
  color: #FFFFFF;
  position: relative;
  left: -100%;
  height: 100%;
  width: 200%;
  transform: translateX(0);
  transition: transform 0.6s ease-in-out;
}

.container.right-panel-active .overlay {
  transform: translateX(50%);
}

.overlay-panel {
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  padding: 0 40px;
  text-align: center;
  top: 0;
  height: 100%;
  width: 50%;
  transform: translateX(0);
  transition: transform 0.6s ease-in-out;
}

.overlay-left {
  transform: translateX(-20%);
}

.container.right-panel-active .overlay-left {
  transform: translateX(0);
}

.overlay-right {
  right: 0;
  transform: translateX(0);
}

.container.right-panel-active .overlay-right {
  transform: translateX(20%);
}

.social-container {
  margin: 20px 0;
}

.social-container a {
  border: 1px solid #DDDDDD;
  border-radius: 50%;
  display: inline-flex;
  justify-content: center;
  align-items: center;
  margin: 0 5px;
  height: 40px;
  width: 40px;
}

footer {
  background-color: #222;
  color: #fff;
  font-size: 14px;
  bottom: 0;
  position: fixed;
  left: 0;
  right: 0;
  text-align: center;
  z-index: 999;
}

footer p {
  margin: 10px 0;
}

footer i {
  color: #FFBD59;
}

footer a {
  color: #3c97bf;
  text-decoration: none;
}


.custom-btn {
  background-color: #FFBD59;
  color: rgb(235, 235, 235);
}

#logoNav {
  width: 50px;
  height: 50px;
}

.navEscolhas {
  font-size: 20px;
}

.nav-item {
  padding-left: 20px;
  margin-right: 20px;
}

h1 {
  color: aliceblue;
  font-size: 30px;
}

.btn {
  margin-right: 20px;
}

.btn:active {
  transform: scale(1.1);
  transition: 0.5s;
}

.login{
  color: grey
}

.logo-text {
  color: white;
  font-size: 30px; 
}

.error-message {
  color: red;
  font-size: 14px;
  margin-top: 10px;
}
</style>