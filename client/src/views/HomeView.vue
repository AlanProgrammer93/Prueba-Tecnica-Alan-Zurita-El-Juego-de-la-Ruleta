<script setup>
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';
import SelectCustom from '../components/SelectCustom.vue'
import ModalAddBalance from '../components/ModalAddBalance.vue'
import ModalWon from '../components/ModalWon.vue'
import { useRouter } from 'vue-router';
import clientAxios from '@/utils/axiosConfig';
import { usePlayStore } from '@/stores/play';
import MessageError from '../components/MessageError.vue'

let loading = ref(false)
let selectOptionColor = ref('')
let selectOptionEvenOdd = ref('')
let betNumber = ref(null)
let betAmount = ref(0)

let showMenu = ref(false)
let showModal = ref(false)
let showModalWon = ref(false)

const messageError = ref('')

const colors = ["", "Negro", "Rojo"]
const evenOdd = ["", "Par", "Impar"]

const router = useRouter();
const authStore = useAuthStore();
const playStore = usePlayStore();

const handlerShow = async () => {
  if (!selectOptionColor.value && !selectOptionEvenOdd.value && !isValidNumber(betNumber.value)) {
    messageError.value = "Debe Seleccionar su Apuesta"
    return
  }

  if (betAmount.value > authStore.user.user.balance) {
    messageError.value = "No Tienes Suficiente Dinero"
    return
  }

  if (betAmount.value < 1) {
    messageError.value = "Debe Ingresar un Monto"
    return
  }

  loading.value = true
  await clientAxios.post('/users/play', {
    Color: selectOptionColor.value,
    EvenOdd: selectOptionEvenOdd.value,
    Number: betNumber.value ? betNumber.value : undefined,
    Amount: betAmount.value,
  }, {
    headers: {
      'Authorization': `Bearer ${authStore.user.token}`
    }
  }).then(res => {
    playStore.setResult({
      color: res.data.color,
      evenOdd: res.data.evenOdd,
      number: res.data.number,
      result: res.data.result,
      won: res.data.won,
    })
    authStore.updateBalance(res.data.user)
  })
    .catch(err => {
      messageError.value = "Ocurrio un Error. Intentelo Otra Ves"
    })
    .finally(resul => loading.value = false)
}

function isValidNumber(value) {
  return value !== null && value !== undefined && Number.isFinite(value) && value >= 0 && value <= 36;
}

const toggleMenu = () => {
  showMenu.value = !showMenu.value
}

const handlerShowModal = () => {
  showModal.value = !showModal.value
  showMenu.value = !showMenu.value
}

const logout = () => {
  authStore.logout()
  router.push('/login');
  localStorage.removeItem("token")
}

</script>

<template>
  <MessageError v-if="messageError" v-model:messageError="messageError" />
  <header>
    <h1>Bienvenido al Juego de la Ruleta</h1>
    <div class="menu">
      <div class="buttonMenu">
        <span>{{ authStore?.user?.user.username }}</span>
        <button @click="toggleMenu">Menu</button>
      </div>

      <div v-if="showMenu" class="options">
        <div class="yourBalance">
          <p>Tu Saldo</p>
          <span>$ {{ authStore?.user?.user.balance }}</span>
        </div>
        <div class="option">
          <p @click="handlerShowModal">Cargar Saldo</p>
        </div>
        <div class="option">
          <p @click="logout">Cerrar Sesion</p>
        </div>
      </div>
    </div>
  </header>
  <div class="container">
    <div :class="`${loading ? 'rouletteMove' : 'roulette'}`">
      <div class="result" v-if="!loading">
        <p>{{ playStore.result.evenOdd }}</p>
        <h1>{{ playStore.result.number }}</h1>
        <p>{{ playStore.result.color }}</p>
      </div>
    </div>
    <div class="betContainer">
      <h2>Realiza Tu Apuesta</h2>
      <div class="betOptions">
        <SelectCustom title="Color" :items="colors" v-model:selected="selectOptionColor" :active=true />
        <SelectCustom title="Par o Impar" :items="evenOdd" v-model:selected="selectOptionEvenOdd"
          :active=!!selectOptionColor />
      </div>
      <div class="betnumber">
        <label>APUESTE A UN NUMERO</label>
        <input type="number" v-model="betNumber" :min="0" :max="36"
          :disabled="!selectOptionColor || (!!selectOptionColor && !!selectOptionEvenOdd)" />
      </div>
      <div class="amount">
        <label>Ingrese un monto</label>
        <input type="number" v-model="betAmount" :min="1" />
      </div>
    </div>
    <button class="btn" :disabled=loading @click="handlerShow">Girar Ruleta</button>
  </div>
  <ModalAddBalance v-if="showModal" v-model:showModal="showModal" />
  <ModalWon v-if="playStore.result.won > 0" v-model:showModalWon="showModalWon" />
</template>

<style scoped>
header {
  background-color: #1f2833;
  padding: 10px;
  position: relative;
}

.yourBalance {
  color: white;
  padding: 8px 10px;
  margin-top: 2px;
  background-color: #53b3ac;
  display: flex;
  flex-direction: column;
  align-items: center;
  user-select: none;
}

.yourBalance span {
  color: green;
  font-size: 22px;
  font-weight: bold;
}

.menu {
  position: absolute;
  top: 50%;
  right: 0;
  transform: translate(-10%, -50%);
}

.buttonMenu {
  display: flex;
  align-items: center;
  gap: 10px;
}

.buttonMenu span {
  color: white;
  user-select: none;
}

.menu button {
  background-color: #66fcf1;
  border: none;
  padding: 5px 8px;
  cursor: pointer;
}

.options {
  position: absolute;
  right: 0;
  width: 130px;
  background-color: #1f2833;
}

.option:hover {
  background-color: #489691;
}

.option {
  color: white;
  padding: 8px 10px;
  margin-top: 2px;
  background-color: #53b3ac;
  cursor: pointer;
}

header h1 {
  font-family: 'Playfair Display', serif;
  color: #66fcf1;
  text-align: center;
  font-size: 26px;
}

.container {
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-between;
}

.roulette {
  width: 300px;
  height: 300px;
  background-image: url('../assets/ruleta.png');
  background-size: cover;
  position: relative;
}

.rouletteMove {
  width: 300px;
  height: 300px;
  background-image: url('../assets/ruleta.png');
  background-size: cover;
  animation: rotation 2s linear infinite;
}

.btn {
  width: 400px;
  background-color: #66fcf1;
  color: #0b0c10;
  padding: 10px 20px;
  border: none;
  cursor: pointer;
  font-size: 18px;
  transition: background-color 0.3s;
  margin-top: 40px;
}

.btn:hover {
  background-color: #45a29e;
}

@keyframes rotation {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}

.result {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
  flex-direction: column;
  align-items: center;
}

.result p {
  font-size: 20px;
  font-weight: bold;
}

.result h1 {
  font-size: 38px;
  font-weight: bold;
  color: green;
}

.betContainer {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 400px;
  margin-top: 60px;
}

.betContainer h2 {
  color: #45a29e;
}

.betOptions {
  display: flex;
  justify-content: space-between;
  gap: 10px;
  width: 100%;
}

.amount {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 10px;
  margin-top: 30px;
}

.amount label {
  font-size: 20px;
  color: #45a29e;
  text-align: center;
  font-weight: bold;
}

.amount input {
  flex: 1;
  border: 2px solid #45a29e;
  background: white;
  height: 32px;
  border-radius: 10px;
  outline: none;
  color: #45a29e;
  padding-left: 15px;
  font-size: 16px;
  font-weight: bold;
}

.betnumber {
  width: 100%;
  display: flex;
  margin-top: 5px;
}

.betnumber label {
  flex: 1;
  text-align: left;
  font-size: 24px;
  color: #45a29e;
  font-weight: bold;
}

.betnumber input {
  border: 2px solid #45a29e;
  background: white;
  height: 34px;
  width: 60px;
  border-radius: 10px;
  outline: none;
  color: #45a29e;
  padding-left: 15px;
  font-size: 16px;
  font-weight: bold;
}
</style>
