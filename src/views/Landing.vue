<template>
  <div class="landing">
    <!-- Hero Start -->
    <section class="d-table overflow-hidden w-100 landing_bg">
      <b-container>
        <b-row class="align-items-center pt-5">
          <b-col md="6" lg="7">
            <div class="title-heading">
              <h1 class="mb-3 title-heading_title">
                One home for all your <br />
                tests and coverage
              </h1>
              <p class="title-heading_text">
                Push directly from any pipeline. Visualize and automate code
                quality. Get started in three simple steps. By the way, it's
                free forever for open source.
              </p>
              <div class="mt-4 pt-2">
                <b-button variant="primary">Getting started</b-button>
              </div>
            </div>
          </b-col>
          <!-- end of col -->

          <b-col lg="5" md="6" class="mt-4 pt-2 mt-sm-0 pt-sm-0">
            <div class="position-relative landing_bg-wrapper">
              <div class="position-relative landing_bg-box">
                <b-img
                  :src="require('@/assets/img/classic01.png')"
                  class="img"
                ></b-img>
              </div>
            </div>
          </b-col>
        </b-row>
      </b-container>
    </section>
    <!-- Hero Start -->

    <!-- Section Start -->
    <section class="overflow-hidden section">
      <!-- Start Features section -->
      <b-container class="pb-5 mb-md-5">
        <b-row>
          <b-col cols="12" md="4">
            <div class="text-center features">
              <div class="d-inline-block position-relative features_icon-box">
                <b-img
                  :src="require('@/assets/img/user.svg')"
                  :height="65"
                  :width="65"
                  center
                ></b-img>
              </div>
              <div class="content mt-4">
                <h4 class="content_title">Easy To Use</h4>
                <p class="content_text mb-0">
                  All it takes is three simple steps to get your pipeline to
                  push to TestHub.
                </p>
              </div>
            </div>
          </b-col>
          <b-col cols="12" md="4" class="mt-5 mt-sm-0">
            <div class="text-center features">
              <div class="d-inline-block position-relative features_icon-box">
                <b-img
                  :src="require('@/assets/img/calendar.svg')"
                  :height="65"
                  :width="65"
                  center
                ></b-img>
              </div>
              <div class="content mt-4">
                <h4 class="content_title">Integrated Control Dashboard</h4>
                <p class="content_text mb-0">
                  Visualize your current and past test and coverage information.
                  Monitor and enforce quality
                </p>
              </div>
            </div>
          </b-col>
          <b-col cols="12" md="4" class="mt-5 mt-sm-0">
            <div class="text-center features">
              <div class="d-inline-block position-relative features_icon-box">
                <b-img
                  :src="require('@/assets/img/sand-clock.svg')"
                  :height="65"
                  :width="65"
                  center
                ></b-img>
              </div>
              <div class="content mt-4">
                <h4 class="content_title">Zero gimmick fees</h4>
                <p class="content_text mb-0">
                  Pay a simple monthly fee for upto ten products. Unlimited
                  builds.
                </p>
              </div>
            </div>
          </b-col>
        </b-row>
      </b-container>
      <!-- End Features section -->

      <!-- Start Statistics section -->
      <b-container class="pb-5 mb-md-5 statistics">
        <b-row class="align-items-center">
          <b-col lg="7">
            <div class="position-relative bubble-shape-left">
              <b-img
                :src="require('@/assets/img/classic02.jpeg')"
                fluid
                rounded
                class="mx-auto d-block shadow"
                alt=""
              />
            </div>
          </b-col>
          <b-col lg="5" class="mt-4 pt-2 mt-lg-0 pt-lg-0">
            <div class="statistics_title ml-lg-4">
              <h1 class="title mb-3">Organisation Level Statistics</h1>
              <p class="text mb-3">
                Analyse test run history, coverage growth dynamics, and test
                coverage analysis. You can view individual project data or a
                cumulative dashboards.
              </p>

              <Carousel
                class="owl-carousel owl-theme"
                :per-page="1"
                :autoplay="true"
              >
                <!-- slide 1 -->
                <Slide>
                  <div class="statistics_customer text-left py-3">
                    <b-avatar
                      :src="require('@/assets/img/01.jpeg')"
                      size="65px"
                      class="shadow"
                    ></b-avatar>
                    <p class="mt-4 text">
                      "TestHub really helped me get a birds eye view on my tests
                      and coverage."
                    </p>
                    <h6 class="text-primary">- Jhonis De Souza</h6>
                  </div>
                </Slide>

                <!-- slide 2 -->
                <Slide>
                  <div class="statistics_customer text-left py-3">
                    <b-avatar
                      :src="require('@/assets/img/02.jpeg')"
                      size="65px"
                      class="shadow"
                    ></b-avatar>
                    <p class="mt-4 text">
                      " TestHub is straightforward. It just does what it says.
                      Full control over your tests and coverage data. "
                    </p>
                    <h6 class="text-primary">- Michelle Gemstone</h6>
                  </div>
                </Slide>

                <!-- slide 3 -->
                <Slide>
                  <div class="statistics_customer text-left py-3">
                    <b-avatar
                      :src="require('@/assets/img/03.jpeg')"
                      size="65px"
                      class="shadow"
                    ></b-avatar>
                    <p class="mt-4 text">
                      " One disadvantage of TestHub is that you don't have to
                      use 10 tools to get the job done. It does everything TEST.
                      "
                    </p>
                    <h6 class="text-primary">- Youssephine Gomatova</h6>
                  </div>
                </Slide>

                <!-- slide 4 -->
                <Slide>
                  <div class="statistics_customer text-left py-3">
                    <b-avatar
                      :src="require('@/assets/img/04.jpeg')"
                      size="65px"
                      class="shadow"
                    ></b-avatar>
                    <p class="mt-4 text">
                      " I didn't know this is the only tool I needed for all
                      this time before I gave TestHub a try. "
                    </p>
                    <h6 class="text-primary">- Alexey Kichin</h6>
                  </div>
                </Slide>
              </Carousel>
            </div>
          </b-col>
        </b-row>
      </b-container>
      <!-- End Statistics section -->

      <!-- Start Choose-us section -->
      <b-container class="choose-us">
        <b-row class="align-items-center">
          <b-col
            lg="5"
            order="2"
            order-lg="1"
            class="mt-4 pt-2 mt-lg-0 pt-lg-0"
          >
            <div class="choose-us_title mr-lg-4">
              <h1 class="title mb-3">Why Choose us ?</h1>
              <p class="text">
                TestHub is the last Test/Coverage tool you'll ever need. With
                tight GitHub integration and great ecosystem, TestHub has a
                roadmap to fulfill all your wishes for a perfect Test and
                Coverage analysis tool.
              </p>
              <b-row>
                <b-col cols="12">
                  <div class="d-flex align-items-center pt-4">
                    <h2>
                      <b-icon icon="shield" variant="primary"></b-icon>
                    </h2>
                    <div class="ml-3">
                      <h5>Fully Managed and provides actionable data</h5>
                      <p class="text mb-0">
                        See what you've been working on daily and how much time
                        you spent on each task or project. You can use automatic
                        or online timer for tracking different tasks.
                      </p>
                    </div>
                  </div>
                </b-col>

                <b-col cols="12">
                  <div class="d-flex align-items-center pt-4">
                    <h2>
                      <b-icon icon="cpu" variant="primary"></b-icon>
                    </h2>
                    <div class="ml-3">
                      <h5>Zero config</h5>
                      <p class="text mb-0">
                        With GitHub actions, TestHub is ready to fly whenever
                        you're ready!
                      </p>
                    </div>
                  </div>
                </b-col>

                <b-col cols="12" class="pt-4">
                  <b-button variant="outline-primary"
                    >Download TestHub CLI Now
                    <b-icon
                      class="icon-margin"
                      font-scale="0.7"
                      icon="chevron-right"
                    ></b-icon>
                  </b-button>
                </b-col>
              </b-row>
            </div>
          </b-col>
          <b-col lg="7" order="1" order-lg="2">
            <div class="bubble-shape-right position-relative">
              <b-img
                :src="require('@/assets/img/classic03.jpeg')"
                fluid
                class="shadow"
              ></b-img>
            </div>
          </b-col>
        </b-row>
      </b-container>
      <!-- End Choose-us section -->

      <!-- Get Started Section -->
      <b-container class="pt-4 get-started">
        <b-row class="justify-content-center">
          <b-col md="8">
            <div class="text-center get-started_title">
              <h4 class="title mb-4">Get Started today for free!</h4>
              <p class="mb-0">
                Monitor your chronological improvement in tests and coverage
                from project level and from organization level. It couldn't have
                been simpler and more effective!
              </p>
            </div>

            <div class="d-flex justify-content-center mt-4">
              <b-button variant="primary" class="mr-4">
                Get Started
                <b-icon
                  class="icon-margin"
                  font-scale="0.7"
                  icon="chevron-right"
                ></b-icon>
              </b-button>
              <div class="watch-video">
                <b-icon
                  font-scale="2.5"
                  icon="play-circle-fill"
                  variant="primary"
                ></b-icon>
                <span class="pl-2">WATCH VIDEO</span>
              </div>
            </div>
          </b-col>
        </b-row>
      </b-container>
      <!-- Get access Section -->

      <!-- Get Started Section -->
      <b-container class="pt-4 access">
        <div class="access_bg rounded p-lg-5 p-4">
          <b-row class="align-items-center">
            <b-col md="8">
              <div class="text-md-left text-center access_title">
                <h4 class="mb-3 title">
                  Get early access to TestHub today for free!
                </h4>
                <p class="mb-0 text">
                  Be a part of TestHub early access program. It's free for open
                  source and beta testers.
                </p>
              </div>
            </b-col>
            <b-col md="4" class="mt-4 mt-sm-0">
              <div class="text-md-right text-center">
                <b-button variant="light">Get Started</b-button>
              </div>
            </b-col>
          </b-row>
        </div>
      </b-container>
      <!-- Get access Section -->
    </section>
  </div>
</template>

<script>
import { Carousel, Slide } from "vue-carousel";

export default {
  components: {
    Carousel,
    Slide,
  },
};
</script>

<style lang="scss" scoped>
.landing{
  background-color: #ffffff !important;
  position: relative;
  z-index: -1;
}
</style>

<style lang="scss">
.landing {
  p {
    color: #6c757d !important;
  }
  .shadow {
    box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important;
  }
  .icon-margin {
    margin-bottom: 0.2rem;
  }
  //Start first hero
  .landing_bg {
    padding: 170px 0;
    background-size: cover;
    position: relative;
    background-position: center center;
    &-wrapper::selection {
      background-color: rgba(47, 85, 212, 0.9);
      color: #ffffff;
    }
    &-box::after {
      box-shadow: 0 0 40px rgba(47, 85, 212, 0.5);
      background-color: #2f55d4;
      content: "";
      z-index: -1;
      position: absolute;
      bottom: 3rem;
      left: 0;
      width: 75rem;
      height: 55rem;
      border-radius: 20rem;
      opacity: 0.9;
      transform: rotate(130deg);
    }
    .title-heading {
      line-height: 26px;
      &_title {
        font-size: 45px;
        letter-spacing: 1px;
      }
      &_text {
        font-size: 18px;
        color: #6c757d;
      }
    }
  }
  //End first hero

  // start of sectio
  .section {
    padding: 100px 0;
    // Start Features section
    .features {
      cursor: pointer;
      &_icon-box::after {
        content: "";
        position: absolute;
        bottom: 5px;
        right: -15px;
        width: 70px;
        height: 70px;
        border-radius: 6px;
        transform: rotate(33.75deg);
        background: linear-gradient(
          45deg,
          transparent,
          rgba(47, 85, 212, 0.09)
        );
      }
      &:hover .features_icon-box::after {
        animation: spin 5s linear infinite;
        background: rgba(47, 85, 212, 0.07);
      }
      .content {
        &_title {
          font-size: 20px;
        }
        &_text {
          color: #6c757d;
        }
      }
    }
    //End Features section

    // Start Statistics section
    .statistics {
      margin-top: 100px;
      .bubble-shape-left::after {
        box-shadow: 0 0 40px rgba(47, 85, 212, 0.1);
        background: rgba(47, 85, 212, 0.1);
        content: "";
        position: absolute;
        z-index: -1;
        left: -10rem;
        bottom: -3rem;
        width: 35rem;
        height: 35rem;
        border-radius: 15rem;
        transform: rotate(145deg);
      }

      &_title {
        .title {
          font-size: 30px;
          letter-spacing: 0.5px;
        }
        .text {
          max-width: 600px;
        }
      }
      // carousell
      .VueCarousel-dot {
        width: 10px !important;
        height: 10px !important;
        margin: 5px 7px !important;
        border-radius: 3px !important;
        transition: all 0.5s ease;
        background: rgba(47, 85, 212, 0.5) !important;
        padding: 0 !important;
        &:focus {
          outline: none !important;
        }
        &-container {
          margin-top: 10px !important;
        }
      }
      .VueCarousel-dot--active {
        background-color: #2f55d4 !important;
        transform: rotate(45deg) !important;
      }
      &_customer {
        cursor: e-resize;
      }
    }
    //End Statistics section

    //Srat Choose-us section
    .choose-us {
      margin-top: 100px;
      &_title {
        .title {
          letter-spacing: 0.5px;
          font-size: 30px;
        }
        .text {
          max-width: 600px;
        }
      }
      .bubble-shape-right {
        &::after {
          box-shadow: 0 0 40px rgba(47, 85, 212, 0.1);
          background: rgba(47, 85, 212, 0.1);
          content: "";
          position: absolute;
          bottom: -3rem;
          right: -10rem;
          width: 35rem;
          height: 35rem;
          border-radius: 15rem;
          transform: rotate(145deg);
          z-index: -1;
        }
      }
    }
    //End Choose-us section

    // start Get started
    .get-started {
      margin-top: 100px;
      .watch-video {
        display: flex;
        align-items: center;
        cursor: pointer;
        font-size: 14px !important;
        //padding: 0.375rem 0.75rem;
        font-size: 1rem;
        line-height: 1.5;
        border-radius: 0.25rem;
      }
    }
    // end Get started

    // start access
    .access {
      margin-top: 100px;
      &_bg {
        background-color: #007bff;
        &.rounded {
          border-radius: 0.25rem;
        }
      }
      &_title {
        .title {
          letter-spacing: 0.5px;
          font-size: 30px;
          color: #fff;
        }
        .text {
          color: rgba(255, 255, 255, 0.5) !important;
        }
      }
    }
    // end access
  }
}

//media sirst section
@media screen and (max-width: 767px) {
  .landing {
    .landing_bg {
      .title-heading_title {
        font-size: 35px;
      }
      &-box {
        .img {
          height: auto;
          max-width: 100%;
        }
        &::after {
          bottom: -5rem;
          left: 10rem;
          width: 30rem;
          height: 30rem;
          border-radius: 10rem;
        }
      }
    }

    .section {
      padding: 60px 0;
      .statistics {
        margin-top: 60px;
        &_title {
          .title {
            font-size: 24px;
          }
        }
      }
      .choose-us {
        margin-top: 60px;
        &_title {
          .title {
            font-size: 24px;
          }
        }
      }
      .get-started {
        margin-top: 60px;
        &_title {
          .title {
            font-size: 24px;
          }
        }
      }
      .access {
        margin-top: 60px;
        &_title {
          .title {
            font-size: 24px;
          }
        }
      }
    }
  }
}

// spin animation
@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
</style>
