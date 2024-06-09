import { defineStore } from 'pinia'

export const usePlayStore = defineStore('play', {
    state: () => ({
        result: {
            color: "",
            evenOdd: "",
            number: 0,
            result: "",
            won: 0
        },
    }),
    actions: {
        setResult(result) {
            this.result = result;
        },
        reset() {
            this.result = {
                color: "",
                evenOdd: "",
                number: 0,
                result: "",
                won: 0
            };
        },
    },
});
