<template>
  <!-- preloader -->
  <pre-loader></pre-loader>
  <!-- header -->
  <header-page></header-page>
  <!-- right-sidebar -->
  <right-side-bar></right-side-bar>
  <!-- left-side-bar -->
  <left-side-bar></left-side-bar>
  <!-- mobile-menu-overlay -->
  <div class="mobile-menu-overlay"></div>
  <!-- main-container -->
  <div class="main-container">
    <router-view />
  </div>
  <div class="footer-wrap pd-20 mb-10 card-box">
    Music App - Bootstrap 4 Admin Template By
    <a href="https://github.com/dropways" target="_blank">Halo Kirin</a>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import PreLoader from "@/components/BaseComponents/PreLoader.vue";
import HeaderPage from "@/components/BaseComponents/HeaderPage.vue";
import RightSideBar from "@/components/BaseComponents/RightSideBar.vue";
import LeftSideBar from "@/components/BaseComponents/LeftSideBar.vue";

export default defineComponent({
  name: "App",
  components: {
    PreLoader,
    HeaderPage,
    RightSideBar,
    LeftSideBar,
  },
});
</script>

<style lang="scss">
</style>
