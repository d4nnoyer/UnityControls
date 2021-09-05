using System;
using System.IO;
using UnityControls.Logging;
using YamlDotNet.RepresentationModel;
using static UnityControls.UnityControls;

namespace UnityControls
{
    class YamlConfigWriter
    {
        internal static  void UpdateConfigFile()
        {
            var stream = 
                new YamlStream(
                new YamlDocument(
                new YamlMappingNode(
                    new YamlScalarNode("behaviors"), new YamlMappingNode(
                        new YamlScalarNode("MoveToGoal"), new YamlMappingNode(
                            new YamlScalarNode("trainer_type"), new YamlScalarNode(trainer_typeValue.Text),
                            new YamlScalarNode("max_steps"), new YamlScalarNode(max_stepsValue.Text),
                            new YamlScalarNode("time_horizon"), new YamlScalarNode(time_horizonValue.Text),
                            new YamlScalarNode("summary_freq"), new YamlScalarNode(summary_freqValue.Text),
                            new YamlScalarNode("threaded"), new YamlScalarNode(threaded.Checked ? "true" : "false"),
                            new YamlScalarNode("hyperparameters"), new YamlMappingNode(
                                new YamlScalarNode("learning_rate"), new YamlScalarNode(learning_rateValue.Text),
                                new YamlScalarNode("beta"), new YamlScalarNode(betaValue.Text),
                                new YamlScalarNode("epsilon"), new YamlScalarNode(epsilonValue.Text),
                                new YamlScalarNode("lambd"), new YamlScalarNode(lambdValue.Text),
                                new YamlScalarNode("num_epoch"), new YamlScalarNode(num_epochValue.Text),
                                new YamlScalarNode("buffer_init_steps"), new YamlScalarNode(buffer_init_stepsValue.Text),
                                new YamlScalarNode("init_entcoef"), new YamlScalarNode(init_entcoefValue.Text),
                                new YamlScalarNode("tau"), new YamlScalarNode(tauValue.Text),
                                new YamlScalarNode("steps_per_update"), new YamlScalarNode(steps_per_updateValue.Text)
                                ),
                            new YamlScalarNode("network_settings"), new YamlMappingNode(
                                new YamlScalarNode("normalize"), new YamlScalarNode(normalize.Checked ? "true" : "false"),
                                new YamlScalarNode("hidden_units"), new YamlScalarNode(hidden_unitsValue.Text),
                                new YamlScalarNode("num_layers"), new YamlScalarNode(num_layersValue.Text)
                                ),
                            new YamlScalarNode("behavioral_cloning"), new YamlMappingNode(
                                new YamlScalarNode("demo_path"), new YamlScalarNode(string.IsNullOrEmpty(demo_pathBCvalue.Text) ? "null" : demo_pathBCvalue.Text),
                                new YamlScalarNode("strength"), new YamlScalarNode(strengthBCvalue.Text),
                                new YamlScalarNode("steps"), new YamlScalarNode(stepsBCvalue.Text),
                                new YamlScalarNode("batch_size"), new YamlScalarNode(batch_sizeBCvalue.Text),
                                new YamlScalarNode("num_epoch"), new YamlScalarNode(num_epochBCvalue.Text)
                                ),
                            new YamlScalarNode("reward_signals"), new YamlMappingNode(
                                new YamlScalarNode("reward_signals"), new YamlMappingNode(
                                    new YamlScalarNode("extrinsic"), new YamlMappingNode(
                                        new YamlScalarNode("strength"), new YamlScalarNode(UnityControls.strengthValue.Text),
                                        new YamlScalarNode("gamma"), new YamlScalarNode(gammaValue.Text)
                                        ),
                                    new YamlScalarNode("gail"), new YamlMappingNode(
                                        new YamlScalarNode("demo_path"), new YamlScalarNode(string.IsNullOrEmpty(demo_pathGAILvalue.Text) ? "null" : demo_pathGAILvalue.Text),
                                        new YamlScalarNode("strength"), new YamlScalarNode(strengthGAILvalue.Text),
                                        new YamlScalarNode("gamma"), new YamlScalarNode(gammaGAILvalue.Text),
                                        new YamlScalarNode("learning_rate"), new YamlScalarNode(learning_rateGAILvalue.Text)
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
                );

            using (TextWriter writer = File.CreateText(configFileFullPath.Text))
            {
                try
                {
                    stream.Save(writer, false);
                }
                catch (Exception)
                {
                    File.Create(Directory.GetCurrentDirectory() + configFileFullPath.Text);
                    stream.Save(writer, false);
                }
            }

            Logger.Log?.Invoke("Configuration changed!");
        }
    }
}
