import Home from './views/Home.vue'
import Project from './views/Project.vue'
import TestRun from './views/TestRun.vue'

export const routes = [
  {
    path: '/',
    name: 'dashboard',
    component: Home,
    meta: { requiresAuth: false }
  },

  {
    path: '/home',
    name: 'dashboard',
    component: Home,
    meta: { requiresAuth: false }
  },

  {
    path: '/projects',
    name: 'projects',
    component: Project,
    meta: { requiresAuth: false }
  },

  {
    path: '/test-run',
    name: 'test-run',
    component: TestRun,
    meta: { requiresAuth: false }
  },

]

