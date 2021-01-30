import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

import Employee from "@/components/Employee";
import Team from "@/components/Team";

const routes = [
    {
        component: Employee,
        name: 'employee',
        path: '/funcionarios'
    },
    {
        component: Team,
        name: 'team',
        path: '/times'
    }
]

export default new VueRouter({routes})