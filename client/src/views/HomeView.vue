<script setup>
import { useAuthStore } from '@/stores/auth';
import { ref } from 'vue';
import SelectCustom from '../components/SelectCustom.vue'

let loading = ref(false)
let selectOptionColor = ref('')
let selectOptionEvenOdd = ref('')
let betNumber = ref(null)
let betAmount = ref(0)

const colors = ["", "Negro", "Rojo"]
const evenOdd = ["", "Par", "Impar"]

const authStore = useAuthStore();

const handlerShow = () => {
  if(!selectOptionColor.value && !selectOptionEvenOdd.value && !isValidNumber(betNumber.value)) {
    console.log("Debe apostar a algo");
    return
  }

  if(betAmount.value < 1) {
    console.log("Debe ingresar un monto");
    return
  }

  loading.value = true
  setTimeout(() => {
    loading.value = false
  }, 5000)
}

function isValidNumber(value) {
  return value !== null && value !== undefined && Number.isFinite(value) && value >= 0 && value <= 36;
}


</script>

<template>
  <header>
    <h1>Bienvenido al Juego de la Ruleta</h1>
  </header>
  <div class="container">
    <div :class="`${loading ? 'rouletteMove' : 'roulette'}`">
      <div class="result" v-if="!loading">
        <p>Par</p>
        <h1>50</h1>
        <p>Color</p>
      </div>
    </div>
    <div class="betContainer">
      <h2>Realiza Tu Apuesta</h2>
      <div class="betOptions">
        <SelectCustom title="Color" :items="colors" v-model:selected="selectOptionColor" />
        <SelectCustom title="Par o Impar" :items="evenOdd" v-model:selected="selectOptionEvenOdd" />
      </div>
      <div class="betnumber">
        <label>APUESTE A UN NUMERO</label>
        <input type="number" v-model="betNumber" :min="0" :max="36" />
      </div>
      <div class="amount">
        <label>Ingrese un monto</label>
        <input type="number" v-model="betAmount" :min="1" />
      </div>
    </div>
    <button class="btn" :disabled=loading @click="handlerShow">Girar Ruleta</button>
  </div>
</template>

<style scoped>
header {
  background-color: #1f2833;
  padding: 10px;
}

header h1 {
  font-family: 'Playfair Display', serif;
  color: #66fcf1;
  margin: 0;
  text-align: center;
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
