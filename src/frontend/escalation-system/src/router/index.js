import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

import Employee from "@/components/Employee";
import Team from "@/components/Team";
import EscalationSchedule from '@/components/EscalationSchedule'
import Incidents from '@/components/Incidents'

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
    },
    {
        component: EscalationSchedule,
        name: 'escalationSchedule',
        path: '/escala-de-acionamento'
    },
    {
        component: Incidents,
        name: 'incidents',
        path: '/incidentes'
    }
]

export default new VueRouter({routes})