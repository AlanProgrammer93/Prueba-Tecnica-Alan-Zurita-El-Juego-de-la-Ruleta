<script setup>
import { useRouter } from 'vue-router'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth';
import clientAxios from '@/utils/axiosConfig';
import MessageError from '../components/MessageError.vue'

const router = useRouter();
const authStore = useAuthStore();
let showLogin = ref(false)

const username = ref('')
const password = ref('')
const confirmPassword = ref('')

const messageError = ref('')

const handlerShow = () => {
  showLogin.value = !showLogin.value
}

const handlerRegister = async () => {
  if(!username.value || !password.value || !confirmPassword.value) {
    messageError.value = "Complete Todos los Campos"
    return
  }
  if(password.value != confirmPassword.value) {
    messageError.value = "Las Contraseñas no Coinciden"
    return
  }
  await clientAxios.post('/users/register', {
    Username: username.value,
    Password: password.value,
  }).then(res => {
    authStore.login(res.data)
    router.push('/');
  })
    .catch(err => {
      messageError.value = "Ocurrio un Error. Intentelo Otra Ves"
    })
}

const handlerLogin = async () => {
  if(!username.value || !password.value) {
    messageError.value = "Complete Todos los Campos"
    return
  }
  await clientAxios.post('/users/login', {
    Username: username.value,
    Password: password.value,
  }).then(res => {
    authStore.login(res.data)
    localStorage.setItem("token", res.data.token)
    router.push('/');
  })
    .catch(err => {
      messageError.value = "Usuario o Contraseña Incorrecta"
    })
}
</script>

<template>
  <div :class="`auth ${showLogin && 'active'}`">

    <MessageError v-if="messageError" v-model:messageError="messageError" />

    <div class="container">
      <div class="blueBg">
        <div class="box signin">
          <h2>Ya tienes una cuenta?</h2>
          <button class="signinBtn" @click="handlerShow">Iniciar</button>
        </div>
        <div class="box signup">
          <h2>No tienes una cuenta?</h2>
          <button class="signupBtn" @click="handlerShow">Registrate Aqui</button>
        </div>
      </div>
      <div :class="`formBx ${showLogin && 'active'}`">
        <div class="form signinForm">
          <div class="formCont">
            <h3>Iniciar Sesion</h3>
            <input type="text" name='username' placeholder='Nombre de Usuario' v-model="username" />
            <input type="password" name='password' placeholder='Contraseña' v-model="password" />
            <button @click="handlerLogin">
              Iniciar Sesion
            </button>
          </div>
        </div>

        <div class="form signupForm">
          <div class="formCont">
            <h3>Registrarse</h3>
            <input type="text" name='username' placeholder='Nombre de Usuario' v-model="username" />
            <input type="password" name='password' placeholder='Contraseña' v-model="password" />
            <input type="password" name='confirmPassword' placeholder='Confirmar Contraseña'
              v-model="confirmPassword" />
            <button @click="handlerRegister">
              Registrarse
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.auth {
  min-width: 100vw;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: deepskyblue;
  transition: 0.5s;
}

.auth.active {
  background-color: rgb(8, 151, 199);
}

.container {
  position: relative;
  width: 800px;
  height: 500px;
  margin: 20px;
}

.blueBg {
  position: absolute;
  top: 40px;
  width: 100%;
  height: 420px;
  display: flex;
  justify-content: center;
  align-items: center;
  background: rgba(255, 255, 255, 0.2);
  box-shadow: 0 5px 45px rgba(0, 0, 0, 0.15);
}

.blueBg .box {
  position: relative;
  width: 50%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
}

.blueBg .box h2 {
  color: #fff;
  font-size: 1.2em;
  font-weight: 500;
  margin-bottom: 10px;
}

.blueBg .box button {
  cursor: pointer;
  padding: 10px 20px;
  background: #fff;
  color: #333;
  font-size: 16px;
  font-weight: 500;
  border: none;
}

.formBx {
  position: absolute;
  top: 0;
  left: 0;
  width: 50%;
  height: 100%;
  background: #fff;
  z-index: 1000;
  display: flex;
  justify-content: center;
  align-items: center;
  box-shadow: 0 5px 45px rgba(0, 0, 0, 0.25);
  transition: 0.5s ease-in-out;
  overflow: hidden;
}

.formBx.active {
  left: 50%;
}

.formBx .form {
  position: absolute;
  left: 0;
  width: 100%;
  padding: 50px;
  transition: 0.5s;
}

.formBx .signinForm {
  transition-delay: 0.25s;
}

.formBx.active .signinForm {
  left: -100%;
  transition-delay: 0;
}

.formBx .signupForm {
  left: 100%;
  transition-delay: 0s;
}

.formBx.active .signupForm {
  left: 0;
  transition-delay: 0.25s;
}

.formBx .form .formCont {
  width: 100%;
  display: flex;
  flex-direction: column;
}

.formBx .form .formCont {
  width: 100%;
  display: flex;
  flex-direction: column;
}

.formBx .form .formCont h3 {
  font-size: 1.5em;
  color: #333;
  margin-bottom: 20px;
  font-weight: 500;
}

.formBx .form .formCont input,
.formBx .form .formCont button {
  width: 100%;
  margin-bottom: 20px;
  padding: 10px;
  outline: none;
  font-size: 16px;
  border: 1px solid #333;
}

.formBx .form .formCont button {
  background: #2b2b2b;
  border: none;
  color: #fff;
  max-width: 120px;
  cursor: pointer;
}

.formBx .form .formCont .forgot {
  color: #333;
}
</style>
