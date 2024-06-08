<script setup>
import { useAuthStore } from '@/stores/auth';
import clientAxios from '@/utils/axiosConfig';
import { ref } from 'vue';

const props = defineProps({
    showModal: Boolean,
});

let amount = ref(0)

const authStore = useAuthStore();
const emits = defineEmits(['update:showModal']);

function handleChange() {
    emits('update:showModal', false);
}

const handlerSubmit = async () => {
    if (amount.value < 1) {
        return
    }
    await clientAxios.post('/users/addBalance', {
        Balance: amount.value,
    }, {
        headers: {
            'Authorization': `Bearer ${authStore.user.token}`
        }
    }).then(res => {
        authStore.updateBalance(res.data)
        amount.value = 0
    })
        .catch(err => console.log(err))
}

</script>

<template>
    <div class="blur">
        <div class="modalContainer">
            <div class="modalHeader">
                <div class="close" @click="handleChange">
                    Salir
                </div>
                <span>Agregar Saldo</span>
            </div>
            <div class="bodyModal">
                <div class="addAmount">
                    <h2>Saldo Disponible</h2>
                    <span>$ {{ authStore.user.user.balance }}</span>
                    <label>Ingrese un monto</label>
                    <input type="number" v-model="amount" :min="1" />
                </div>
                <button class="btn" @click="handlerSubmit">Aceptar</button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.blur {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    background: rgba(255, 255, 255, 0.768);
    bottom: 0;
    z-index: 9999999999;
}

.modalContainer {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    background: white;
    box-shadow: 0 12px 28px 0 rgba(0, 0, 0, 0.2), 0 2px 4px 0 rgba(0, 0, 0, 0.1),
        inset 0 0 0 1px rgba(255, 255, 255, 0.5);
    width: 400px;
    border-radius: 5px;
}

.modalHeader {
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 20px;
    font-weight: 700;
    padding: 14px 15px 17px 15px;
    background-color: #1f2833;
    color: #66fcf1;
}

.modalHeader .close {
    position: absolute;
    top: 50%;
    right: 0;
    transform: translate(-50%, -50%);
    cursor: pointer;
    font-size: 16px;
}

.bodyModal {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.addAmount {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 10px;
    margin-top: 30px;
}

.addAmount h2 {
    color: #45a29e;
}

.addAmount span {
    color: green;
    font-size: 28px;
    font-weight: bold;
}

.addAmount label {
    font-size: 26px;
    color: #45a29e;
    font-weight: bold;
    margin-top: 10px;
}

.addAmount input {
    flex: 1;
    border: 2px solid #45a29e;
    background: white;
    border-radius: 10px;
    outline: none;
    color: #45a29e;
    padding-left: 15px;
    font-size: 16px;
    font-weight: bold;
}

.btn {
    background-color: #66fcf1;
    color: #0b0c10;
    padding: 8px 15px;
    border: none;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 0.3s;
    margin-top: 40px;
    width: 180px;
    margin-bottom: 50px;
}

.btn:hover {
    background-color: #45a29e;
}
</style>