import Home from './views/Home.vue'
import ProjectRuns from './views/ProjectRuns.vue'
import TestRun from './views/TestRun.vue'

export const routes = [
    {
        path: '/',
        name: 'dashboard',
        component: Home,
        meta: {requiresAuth: false}
    },
    {
        path: '/home',
        name: 'dashboard',
        component: Home,
        meta: {requiresAuth: false}
    }, {
        path: '/:org',
        name: 'org-home',
        component: Home,
        meta: {requiresAuth: false}
    },
    {
        path: '/:org/projects/:project/runs',
        name: 'project-test-runs',
        component: ProjectRuns,
        meta: {requiresAuth: false}
    },
    {
        path: '/:org/projects/:project/runs/{run}',
        name: 'test-run',
        component: TestRun,
        meta: {requiresAuth: false}
    },
]

