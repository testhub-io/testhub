import Home from "./views/Home.vue";
import ProjectRuns from "./views/ProjectRuns.vue";
import TestRun from "./views/TestRun.vue";
import Landing from "./views/Landing.vue";
import TestGrid from "./views/TestGrid.vue";

export const routes = [
  {
    path: "/",
    name: "landing",
    component: Landing,
    meta: { requiresAuth: false },
  },
  {
    path: "/dashboard",
    name: "dashboard",
    component: Home,
    meta: { requiresAuth: false },
  },
  {
    path: "/:org",
    name: "org-home",
    component: Home,
    meta: { requiresAuth: false },
  },
  {
    path: "/:org/projects/:project/runs",
    name: "project-test-runs",
    component: ProjectRuns,
    meta: { requiresAuth: false },
  },
  {
    path: "/:org/projects/:project/runs/:run",
    name: "test-run",
    component: TestRun,
    meta: { requiresAuth: false },
  },
  {
    path: "/:org/projects/:project/tests",
    name: "test-grid",
    component: TestGrid,
    meta: { requiresAuth: false },
  },
];
