const state = {
  codeFile: null,
  baseRunUrl: null,
};

const actions = {
  setCodeFile({ commit }, codeFile) {
    commit('SET_CURRENT_CODE_FILE', codeFile)
  },

  setBaseRunUrl({ commit }, url) {
    commit('SET_BASE_RUN_URL', url)
  }
};

const mutations = {
  SET_CURRENT_CODE_FILE (state, codeFile) {
    state.codeFile = codeFile
  },

  SET_BASE_RUN_URL (state, url) {
    state.baseRunUrl = url
  }
};

const getters = {
  codeFile: () => state.codeFile,
  baseRunUrl: () => state.baseRunUrl
};

export default { state, actions, mutations, getters };
