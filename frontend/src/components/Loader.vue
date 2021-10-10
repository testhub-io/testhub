<template>
  <b-container class="loader-wrapper">
    <div class="loader"></div>
  </b-container>
</template>

<script>
export default {
}
</script>

<style>
.loader {
  -webkit-animation: spin 1s linear infinite;
  animation: spin 1s linear infinite;
  border: 3px solid #ddd;
  border-top: 3px solid #007bff;
  border-radius: 50%;
  height: 75px;
  width: 75px;
  margin: 5px auto;
}

@-webkit-keyframes spin {
  to {
    -webkit-transform: rotate(360deg);
            transform: rotate(360deg);
  }
}

@keyframes spin {
  to {
    -webkit-transform: rotate(360deg);
            transform: rotate(360deg);
  }
}

</style>
