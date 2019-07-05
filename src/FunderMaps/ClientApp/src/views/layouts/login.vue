<template>
  <div class="LoginLayout d-flex justify-content-center">
    <FadedBackgroundCover image="login/background.png" />
    <div class="LoginLayout__modal align-self-center d-flex flex-wrap justify-content-center">
      <LoginBranding />
      <div class="LoginLayout__content p-5">
        <slot />
      </div>
    </div>
  </div>
</template>

<script>
import LoginBranding from 'molecule/LoginBranding'
import FadedBackgroundCover from 'atom/FadedBackgroundCover'

export default {
  name: 'login',
  components: {
    LoginBranding, FadedBackgroundCover
  }
}
</script>

<style lang="scss">
.LoginLayout {
  position: relative;
  height: 100%;
  width: 100%;

  &__modal {
    position: relative;
    border-radius: 5px;
    overflow: hidden;
  }
  &__content {
    background: white;
    width: 480px;
    height: 480px;
  }
}
</style>

