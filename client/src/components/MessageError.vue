<script setup>
const props = defineProps({
    messageError: String,
});

const emits = defineEmits(['update:messageError']);

function closeAlert() {
    emits('update:messageError', "");
}

</script>

<template>
    <div :class="['alert', 'error']">
        <p>{{ messageError }}</p>
        <button @click="closeAlert" class="close-button">X</button>
    </div>
</template>

<style>
.alert {
    position: absolute;
    top: 15px;
    right: 10px;
    padding: 10px 30px;
    margin-bottom: 15px;
    border-radius: 5px;
    font-size: 16px;
    display: flex;
    gap: 15px;
    z-index: 9999;
}

.error {
    background-color: #f8d7da;
    border-color: #f5c6cb;
    color: #721c24;
}

.close-button {
    float: right;
    background: none;
    border: none;
    color: inherit;
    font-size: 16px;
    cursor: pointer;
}
</style>