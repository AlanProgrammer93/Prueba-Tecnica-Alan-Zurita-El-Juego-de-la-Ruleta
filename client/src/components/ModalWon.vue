<script setup>
import { useAuthStore } from '@/stores/auth';
import { usePlayStore } from '@/stores/play';
import clientAxios from '@/utils/axiosConfig';

const props = defineProps({
    showModalWon: Boolean,
});

const authStore = useAuthStore();
const playStore = usePlayStore();

const emits = defineEmits(['update:showModalWon']);

function handleChange() {
    playStore.reset()
    emits('update:showModalWon', false);
}

const handlerSubmitWon = async () => {
    await clientAxios.post('/users/addBalance', {
        Balance: playStore.result.won,
    }, {
        headers: {
            'Authorization': `Bearer ${authStore.user.token}`
        }
    }).then(res => {
        authStore.updateBalance(res.data)
        playStore.reset()
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
                <span>GANASTE!</span>
            </div>
            <div class="bodyModal">
                <div class="addAmount">
                    <h2>Saldo Disponible</h2>
                    <span>${{ authStore.user.user.balance }}</span>
                    <label>Ganaste ${{ playStore.result.won }}</label>
                </div>
                <button class="btn" @click="handlerSubmitWon">Recibir Premio</button>
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
    font-size: 28px;
    color: #45a29e;
    font-weight: bold;
    margin-top: 15px;
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