<script setup>
import { defineProps, defineEmits, ref, watch } from 'vue';

const props = defineProps({
    title: String,
    items: Array,
    selected: String
});

const emits = defineEmits(['update:selected']);

const selectedValue = ref(props.selected);

watch(selectedValue, (newValue) => {
    emits('update:selected', newValue);
});

function handleChange() {
    console.log(selectedValue.value);
    emits('update:selected', selectedValue.value);
}

</script>

<template>
    <div className='selectContainer'>
        <h2>{{ props.title }}</h2>
        <select v-model="selectedValue" @change="handleChange">
            <option v-for="(item, index) in props.items" :key="item" :value="item">{{ item }}</option>
        </select>
    </div>
</template>

<style scoped>
.selectContainer {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 5px;
    margin-top: 20px;
    margin-bottom: 10px;
}

.selectContainer h2 {
    font-size: 20px;
    color: #45a29e;
    text-align: center
}

.selectContainer select {
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

</style>