import { defineStore } from 'pinia'
// { name: "Alan", monto: 2000 }
export const useAuthStore = defineStore('auth', {
    state: () => ({
        user: null,
    }),
    actions: {
        login(userData) {
            this.user = userData;
        },
        logout() {
            this.user = null;
        },
        updateBalance(userUpdated) {
            this.user.user = userUpdated;
        }
    },
    getters: {
        getUser(state) {
            return state.user;
        }
    }
});